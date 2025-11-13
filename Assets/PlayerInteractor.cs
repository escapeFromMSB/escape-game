using UnityEngine;

[DefaultExecutionOrder(-10)]
public class PlayerInteractor : MonoBehaviour
{
    [Header("View / Targeting")]
    public Camera viewCamera;                 // if null, falls back to Camera.main
    public float interactDistance = 3.0f;
    public LayerMask interactMask = ~0;       // everything by default
    public float losRadius = 0.2f;            // for spherecast forgiveness

    [Header("Prompt")]
    public string promptFormat = "Press R to pick up {0}";
    public KeyCode interactKey = KeyCode.R;
    public KeyCode inventoryKey = KeyCode.I;

    // runtime
    private InventoryPickup currentPickup;
    private bool canPickup;
    private InventoryUI invUI;

    // IMGUI style (created inside OnGUI only — never touch GUI.* in Start/Awake)
    private GUIStyle promptStyle;
    private GUIStyle promptShadowStyle;

    void Start()
    {
        // Camera
        if (!viewCamera) viewCamera = Camera.main;

        // Make sure an InventoryUI exists so I toggles reliably
        invUI = FindObjectOfType<InventoryUI>();
        if (!invUI)
        {
            var go = new GameObject("InventoryUI");
            invUI = go.AddComponent<InventoryUI>();
        }
    }

    void Update()
    {
        if (!viewCamera) { viewCamera = Camera.main; }

        // Toggle inventory
        if (Input.GetKeyDown(inventoryKey) && invUI != null)
        {
            invUI.Toggle();
        }

        // Find pickup under crosshair (center of screen)
        var center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = viewCamera != null
            ? viewCamera.ScreenPointToRay(center)
            : new Ray(transform.position + Vector3.up * 1.5f, transform.forward);

        bool hadTarget = currentPickup != null;

        // Spherecast for friendlier targeting and LOS against walls
        if (Physics.SphereCast(ray, losRadius, out RaycastHit hit, interactDistance, interactMask, QueryTriggerInteraction.Ignore))
        {
            var pickup = hit.collider.GetComponentInParent<InventoryPickup>();
            if (pickup != null)
            {
                // New target?
                if (pickup != currentPickup)
                {
                    // clear old pulse
                    if (currentPickup != null) currentPickup.SetHoverEligible(false);
                    currentPickup = pickup;
                }

                // Eligible now -> enable pulse
                currentPickup.SetHoverEligible(true);
                canPickup = true;

                // Pickup input
                if (Input.GetKeyDown(interactKey))
                {
                    var pi = FindObjectOfType<PlayerInventory>();
                    if (!pi)
                    {
                        var holder = new GameObject("PlayerInventory");
                        pi = holder.AddComponent<PlayerInventory>();
                    }

                    // Try add to inventory
                    bool added = pi.TryAddItem(currentPickup.itemId, currentPickup.displayName, currentPickup.amount);
                    if (added)
                    {
                        // consume the world object
                        Destroy(currentPickup.gameObject);
                        currentPickup = null;
                        canPickup = false;
                    }
                    else
                    {
                        // Could show a small message or play a sound here if you like
                    }
                }
            }
            else
            {
                // Hit something else: clear
                ClearCurrent();
            }
        }
        else
        {
            // Nothing in front
            ClearCurrent();
        }
    }

    private void ClearCurrent()
    {
        if (currentPickup != null)
        {
            currentPickup.SetHoverEligible(false); // stops pulse immediately
            currentPickup = null;
        }
        canPickup = false;
    }

    void OnGUI()
    {
        // Create styles lazily INSIDE OnGUI so we never touch GUI.skin in Start/Awake
        if (promptStyle == null)
        {
            promptStyle = new GUIStyle(GUI.skin.label);
            promptStyle.alignment = TextAnchor.MiddleCenter;
            promptStyle.fontSize = 18;
            promptStyle.normal.textColor = Color.white;

            promptShadowStyle = new GUIStyle(promptStyle);
            promptShadowStyle.normal.textColor = new Color(0, 0, 0, 0.6f);
        }

        if (canPickup && currentPickup != null)
        {
            string msg = string.Format(promptFormat, currentPickup.displayName);

            float w = 480f;
            float h = 30f;
            float x = (Screen.width - w) * 0.5f;
            float y = Screen.height * 0.5f + 40f; // slightly below crosshair

            // Simple shadow
            Rect rShadow = new Rect(x + 1f, y + 1f, w, h);
            GUI.Label(rShadow, msg, promptShadowStyle);

            Rect r = new Rect(x, y, w, h);
            GUI.Label(r, msg, promptStyle);
        }
    }
}
