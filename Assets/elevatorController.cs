using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Targets & Speed")]
    public Transform platformTarget;                   
    [SerializeField] private float speed = 1.5f;       
    [SerializeField] private float travelDistance = 3f; 

    [Header("Player Detection")]
    [SerializeField] private string playerTag = "";     

    [Header("Power Gating")]
    [SerializeField] private bool requirePower = true;

    [Header("Computed")]
    [SerializeField] private Vector3 topPos;
    [SerializeField] private Vector3 bottomPos;

    private bool playerInZone = false;
    private bool moving = false;
    private bool goingDown = false;

    void Start()
    {
        if (platformTarget == null)
            platformTarget = transform;

        topPos = platformTarget.position;
        bottomPos = topPos + new Vector3(0f, -Mathf.Abs(travelDistance), 0f);
    }

    void Update()
    {
        if (playerInZone && !moving && Input.GetKeyDown(KeyCode.E))
        {
            TryStartRide();
        }

        if (!moving) return;

        Vector3 target = goingDown ? bottomPos : topPos;
        platformTarget.position = Vector3.MoveTowards(platformTarget.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(platformTarget.position, target) < 0.001f)
        {
            platformTarget.position = target;
            moving = false;
            Debug.Log($"[Elevator] Arrived at {(goingDown ? "BOTTOM" : "TOP")}. Awaiting next E press.");
        }
    }

    private void TryStartRide()
    {
        if (requirePower && !BuildingPower.IsOn)
        {
            CenterMessage.Show("The elevator has no power.", 1.5f);
            return;
        }
        bool atTop = IsAtTop();
        bool atBottom = IsAtBottom();

        
        if (!atTop && !atBottom)
        {
            Debug.Log("[Elevator] Ignored: platform is between floors.");
            return;
        }

        goingDown = atTop;
        moving = true;

        Debug.Log($"[Elevator] Starting ride {(goingDown ? "DOWN" : "UP")}.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInZone = true;
            Debug.Log("[Elevator] Player entered zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInZone = false;
            Debug.Log("[Elevator] Player exited zone.");
        }
    }

    private bool IsPlayer(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        return other.GetComponent<CharacterController>() != null || other.attachedRigidbody != null;
    }

    private bool IsAtTop()    => Vector3.Distance(platformTarget.position, topPos)    < 0.01f;
    private bool IsAtBottom() => Vector3.Distance(platformTarget.position, bottomPos) < 0.01f;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 top = (platformTarget ? platformTarget.position : transform.position);
        Vector3 bottom = top + new Vector3(0f, -Mathf.Abs(travelDistance), 0f);

        Gizmos.DrawWireSphere(top, 0.1f);
        Gizmos.DrawWireSphere(bottom, 0.1f);
    }
#endif
}
