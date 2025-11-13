using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Targets & Speed")]
    public Transform platformTarget;                   // The object that actually moves
    [SerializeField] private float speed = 1.5f;       // Units per second
    [SerializeField] private float travelDistance = 3f; // How far to travel vertically (down)

    [Header("Player Detection")]
    [SerializeField] private string playerTag = "";     // Optional: set to "Player" to be strict

    [Header("Computed")]
    [SerializeField] private Vector3 topPos;
    [SerializeField] private Vector3 bottomPos;

    [Header("State (read-only)")]
    [SerializeField] private bool moving = false;
    [SerializeField] private bool goingDown = false;    // true → toward bottomPos, false → toward topPos
    [SerializeField] private bool playerInZone = false;

    private Collider trig;                 // our trigger collider
    private Collider currentPlayer;        // who is inside

    void Start()
    {
        if (platformTarget == null)
        {
            platformTarget = transform;
            Debug.LogWarning("[Elevator] platformTarget not set; using this.transform");
        }

        // Define endpoints (top = current position; bottom = down by travelDistance)
        topPos = platformTarget.position;
        bottomPos = topPos + new Vector3(0f, -Mathf.Abs(travelDistance), 0f);

        // Ensure we have a trigger
        trig = GetComponent<Collider>();
        if (trig == null)
            Debug.LogError("[Elevator] No Collider found. Add a BoxCollider and check 'Is Trigger'.");
        else if (!trig.isTrigger)
            Debug.LogWarning("[Elevator] Collider exists but Is Trigger is OFF. Turn it ON.");

        if (speed <= 0f) Debug.LogWarning("[Elevator] Speed <= 0.");

        moving = false;
        goingDown = false;

        Debug.Log($"[Elevator] Ready. Top={topPos}, Bottom={bottomPos}");
    }

    void Update()
    {
        // Handle interaction input
        if (playerInZone && !moving && Input.GetKeyDown(KeyCode.E))
        {
            TryStartRide();
        }

        // Handle movement
        if (!moving) return;

        Vector3 target = goingDown ? bottomPos : topPos;
        platformTarget.position = Vector3.MoveTowards(platformTarget.position, target, speed * Time.deltaTime);

        // Arrived?
        if (Vector3.Distance(platformTarget.position, target) <= 0.001f)
        {
            platformTarget.position = target; // snap to exact
            moving = false;
            Debug.Log($"[Elevator] Arrived at {(goingDown ? "BOTTOM" : "TOP")}. Awaiting next E press.");
        }
    }

    private void TryStartRide()
    {
        bool atTop = IsAtTop();
        bool atBottom = IsAtBottom();

        // Only allow starting from endpoints
        if (!atTop && !atBottom)
        {
            Debug.Log("[Elevator] Ignored: platform is between floors.");
            return;
        }

        // Decide direction:
        // At top -> go down; At bottom -> go up
        goingDown = atTop;
        moving = true;

        Debug.Log($"[Elevator] E pressed → moving {(goingDown ? "DOWN" : "UP")}.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        playerInZone = true;
        currentPlayer = other;
        // (Optional) You can show a separate UI hint via your own UI system here.
        // Debug.Log("[Elevator] Player entered zone. Press E to ride.");
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        if (currentPlayer != null && other != currentPlayer) return;

        playerInZone = false;
        currentPlayer = null;
        // Debug.Log("[Elevator] Player left zone.");
    }

    // -------- Helpers --------
    private bool IsPlayer(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        // Heuristic fallback: CharacterController or Rigidbody feels “player-like”
        return other.GetComponent<CharacterController>() != null || other.attachedRigidbody != null;
    }

    private bool IsAtTop()    => Vector3.Distance(platformTarget.position, topPos)    < 0.01f;
    private bool IsAtBottom() => Vector3.Distance(platformTarget.position, bottomPos) < 0.01f;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(topPos, 0.08f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(bottomPos, 0.08f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(topPos, bottomPos);
    }
#endif
}
