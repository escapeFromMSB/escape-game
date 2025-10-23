using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DoorAutoOpen : MonoBehaviour
{
    // ----- References (wired from BuildDoor.Initialize) -----
    [SerializeField] private Transform doorVisual;        // the cube that looks like the door
    [SerializeField] private Collider  blockingCollider;  // the door's blocking collider

    // ----- Hinge / Motion -----
    [Header("Hinge / Motion")]
    [SerializeField] private float openAngle  = 90f;      // degrees from closed
    [SerializeField] private float openSpeed  = 180f;     // deg/sec opening
    [SerializeField] private float closeSpeed = 180f;     // deg/sec closing
    [SerializeField] private bool  openClockwise = false; // flip if it swings the wrong way

    // ----- Trigger sizing -----
    [Header("Trigger Size Tuning")]
    [Tooltip("If true, use the custom size/center below; otherwise auto-size from door.")]
    [SerializeField] private bool   useCustomTrigger     = false;
    [SerializeField] private Vector3 customTriggerSize   = new Vector3(8f, 3f, 10f);
    [SerializeField] private Vector3 customTriggerCenter = new Vector3(0f, 0f, 1.2f);

    [Tooltip("Used when not using custom size. Width multiplier for door width.")]
    [SerializeField] private float triggerWidthMultiplier  = 6.0f;
    [Tooltip("Used when not using custom size. Depth multiplier for door thickness.")]
    [SerializeField] private float triggerDepthMultiplier  = 15.0f;
    [Tooltip("Used when not using custom size. Extra height beyond door height.")]
    [SerializeField] private float triggerHeightPadding    = 1.25f;
    [Tooltip("Forward bias so it opens earlier from the approach side (local +Z).")]
    [SerializeField] private float triggerForwardBiasZ     = 0.0f;

    // ----- Collision re-enable -----
    [Header("Blocking Collider")]
    [Tooltip("Door blocks again when this close to shut (degrees from closed).")]
    [SerializeField] private float reEnableColliderAt = 10f;

    // ----- Player detection -----
    [Header("Detection")]
    [Tooltip("Leave empty to detect CharacterController automatically.")]
    [SerializeField] private string playerTag = "";

    // ----- Anti-flap (hysteresis) -----
    [Header("Hysteresis")]
    [Tooltip("Stay open this long after the player leaves the trigger.")]
    [SerializeField] private float exitGraceSeconds = 0.35f;
    [Tooltip("Once opened, don't allow closing earlier than this.")]
    [SerializeField] private float minOpenSeconds   = 0.45f;

    // ----- Private state -----
    private Quaternion closedRot, targetRot;
    private int overlapCount;
    private float lastSeenAt = -999f;
    private float openedAt   = -999f;
    private bool  openWanted = false;

    // Call this right after AddComponent in BuildDoor
    public void Initialize(Transform visual, Collider blocking, bool openCW = false)
    {
        doorVisual      = visual;
        blockingCollider = blocking;
        openClockwise   = openCW;

        // --- Trigger setup ---
        var trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;

        // Ensure kinematic Rigidbody so triggers fire with CharacterController
        var rb = gameObject.GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity  = false;

        closedRot = transform.rotation;
        targetRot = closedRot;

        // Auto-size trigger based on door's renderer, unless using custom
        var rend = doorVisual ? doorVisual.GetComponent<Renderer>() : null;
        var size = rend ? rend.bounds.size : new Vector3(1f, 2f, 0.2f);

        if (useCustomTrigger)
        {
            trigger.center = customTriggerCenter;
            trigger.size   = customTriggerSize;
        }
        else
        {
            trigger.center = new Vector3(0f, 0f, triggerForwardBiasZ);
            trigger.size   = new Vector3(
                Mathf.Max(size.x * triggerWidthMultiplier, 2.5f), // wider helps angled approaches
                size.y + triggerHeightPadding,                    // a bit taller
                size.z * triggerDepthMultiplier                   // deeper so it opens earlier
            );
        }
    }

    void Update()
    {
        // --- Hysteresis decision ---
        bool presence     = overlapCount > 0;
        bool seenRecently = (Time.time - lastSeenAt) <= exitGraceSeconds;

        bool newOpenWanted = presence || seenRecently;

        // enforce minimum time open once we start opening
        if (newOpenWanted && !openWanted)
            openedAt = Time.time;
        if (!newOpenWanted && (Time.time - openedAt) < minOpenSeconds)
            newOpenWanted = true;

        openWanted = newOpenWanted;

        // --- Rotate toward target ---
        float sign = openClockwise ? -1f : 1f;
        targetRot = openWanted
            ? closedRot * Quaternion.Euler(0f, sign * openAngle, 0f)
            : closedRot;

        float speed = openWanted ? openSpeed : closeSpeed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);

        // --- Toggle blocking collider (avoid snagging mid-swing) ---
        if (blockingCollider)
        {
            float angFromClosed = Quaternion.Angle(transform.rotation, closedRot);
            bool shouldBlock = angFromClosed <= reEnableColliderAt;
            if (blockingCollider.enabled != shouldBlock)
                blockingCollider.enabled = shouldBlock;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            overlapCount++;
            lastSeenAt = Time.time;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other))
        {
            lastSeenAt = Time.time; // keep "seen" fresh while grazing the edge
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            overlapCount = Mathf.Max(0, overlapCount - 1);
            // lastSeenAt not changed; grace timer handles it
        }
    }

    private bool IsPlayer(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        // Fallback: accept anything with a CharacterController
        return other.GetComponent<CharacterController>() != null;
    }

    // Visualize the trigger for tuning
    void OnDrawGizmosSelected()
    {
        var bc = GetComponent<BoxCollider>();
        if (!bc) return;

        Gizmos.color = new Color(0f, 1f, 1f, 0.2f);
        var m = Matrix4x4.TRS(transform.TransformPoint(bc.center), transform.rotation, transform.lossyScale);
        Gizmos.matrix = m;
        Gizmos.DrawCube(Vector3.zero, bc.size);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, bc.size);
    }
}
