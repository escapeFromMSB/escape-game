using UnityEngine;

[DefaultExecutionOrder(-10)]
public class PlayerInteractor : MonoBehaviour
{
    [Header("View / Targeting")]
    public Camera viewCamera;                 
    public float interactDistance = 3.0f;
    public LayerMask interactMask = ~0;       
    public float losRadius = 0.2f;            

    [Header("Prompt")]
    public string promptFormat = "Press R to pick up {0}";
    public KeyCode interactKey = KeyCode.R;
    public KeyCode inventoryKey = KeyCode.I;

    
    private InventoryPickup currentPickup;
    private bool canPickup;
    private InventoryUI invUI;

    private GUIStyle promptStyle;
    private GUIStyle promptShadowStyle;

    void Start()
    {
        if (!viewCamera) viewCamera = Camera.main;
        
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
        
        if (Input.GetKeyDown(inventoryKey) && invUI != null)
        {
            invUI.Toggle();
        }
        
        var center = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = viewCamera != null
            ? viewCamera.ScreenPointToRay(center)
            : new Ray(transform.position + Vector3.up * 1.5f, transform.forward);

        bool hadTarget = currentPickup != null;
        
        if (Physics.SphereCast(ray, losRadius, out RaycastHit hit, interactDistance, interactMask, QueryTriggerInteraction.Ignore))
        {
            var pickup = hit.collider.GetComponentInParent<InventoryPickup>();
            if (pickup != null)
            {
                if (pickup != currentPickup)
                {
                    if (currentPickup != null) currentPickup.SetHoverEligible(false);
                    currentPickup = pickup;
                }
                
                currentPickup.SetHoverEligible(true);
                canPickup = true;
                
                if (Input.GetKeyDown(interactKey))
                {
                    var pi = FindObjectOfType<PlayerInventory>();
                    if (!pi)
                    {
                        var holder = new GameObject("PlayerInventory");
                        pi = holder.AddComponent<PlayerInventory>();
                    }
                    
                    bool added = pi.TryAddItem(currentPickup.itemId, currentPickup.displayName, currentPickup.amount);
                    if (added)
                    {
                        Destroy(currentPickup.gameObject);
                        currentPickup = null;
                        canPickup = false;
                    }
                }
            }
            else
            {
                ClearCurrent();
            }
        }
        else
        {
            ClearCurrent();
        }
    }

    private void ClearCurrent()
    {
        if (currentPickup != null)
        {
            currentPickup.SetHoverEligible(false); 
            currentPickup = null;
        }
        canPickup = false;
    }

    void OnGUI()
    {
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
            float y = Screen.height * 0.5f + 40f; 

           Rect rShadow = new Rect(x + 1f, y + 1f, w, h);
            GUI.Label(rShadow, msg, promptShadowStyle);

            Rect r = new Rect(x, y, w, h);
            GUI.Label(r, msg, promptStyle);
        }
    }
}
