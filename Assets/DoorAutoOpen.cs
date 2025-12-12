using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DoorAutoOpen : MonoBehaviour
{
    [Header("Door pieces")]
    [Tooltip("Child transform that visually swings open/closed.")]
    public Transform doorVisual;        

    [Tooltip("Solid collider on the door mesh that blocks the player.")]
    public Collider blockingCollider;   

    [Header("Opening")]
    public bool openClockwise = true;
    public float openAngle = 90f;
    public float openSpeed = 4f;

    [Header("Hysteresis (same idea as elevator doors)")]
    [Tooltip("How long to stay open after the player leaves the trigger.")]
    public float exitGraceSeconds = 0.75f;

    [Tooltip("Once opened, keep door open at least this long.")]
    public float minOpenSeconds = 0.25f;

    [Tooltip("Optional. If empty, we detect the player via CharacterController like the elevator.")]
    public string playerTag = "";

    
    Quaternion _closedLocalRot;
    Quaternion _openLocalRot;

    BoxCollider _trigger;

    bool _opening = false;
    float _lastSeenAt = -999f;
    float _openedAt   = -999f;

    void Reset()
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.isTrigger = true;
    }

    void Awake()
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.isTrigger = true;

        
        if (doorVisual == null && transform.childCount > 0)
            doorVisual = transform.GetChild(0);

        if (doorVisual == null)
        {
            Debug.LogError("DoorAutoOpen on " + name + " has no doorVisual assigned.");
            enabled = false;
            return;
        }

        
        if (blockingCollider == null)
            blockingCollider = doorVisual.GetComponent<Collider>();

        
        if (blockingCollider == _trigger)
        {
            Debug.LogWarning("DoorAutoOpen on " + name +
                             " has trigger collider set as blockingCollider. Clearing.");
            blockingCollider = null;
        }

        
        
        _closedLocalRot = transform.localRotation;
        float sign = openClockwise ? 1f : -1f;
        _openLocalRot = _closedLocalRot * Quaternion.Euler(0f, sign * openAngle, 0f);
    }

    void OnEnable()
    {
        
        _opening = false;
        _lastSeenAt = -999f;
        _openedAt   = -999f;

        transform.localRotation = _closedLocalRot;

        if (blockingCollider != null)
            blockingCollider.enabled = true;
    }

    bool IsPlayer(Collider other)
    {
        
        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        
        return other.GetComponent<CharacterController>() != null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        _lastSeenAt = Time.time;
    }

    void OnTriggerStay(Collider other)
    {
        if (!IsPlayer(other)) return;
        _lastSeenAt = Time.time;
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        
        _lastSeenAt = Time.time;
    }

    void Update()
    {
        
        bool shouldOpen = false;

        
        if (Time.time - _lastSeenAt <= exitGraceSeconds)
            shouldOpen = true;

        
        if (Time.time - _openedAt <= minOpenSeconds)
            shouldOpen = true;

        
        if (!_opening && shouldOpen)
            _openedAt = Time.time;

        _opening = shouldOpen;

        
        Quaternion target = _opening ? _openLocalRot : _closedLocalRot;
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            target,
            Time.deltaTime * openSpeed
        );

        
        if (blockingCollider != null)
            blockingCollider.enabled = !_opening;
    }
}
