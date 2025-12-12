using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(DoorAutoOpen))]
public class KeycardPuzzleDoorLock : MonoBehaviour
{
    [TextArea]
    public string lockedMessage = "Door locked.\nMaybe those colored keycard terminals will unlock it.";

    private DoorAutoOpen _auto;
    private bool _wasLocked;

    void Reset()
    {
        
        var c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    void Awake()
    {
        _auto = GetComponent<DoorAutoOpen>();
    }

    void Start()
    {
        UpdateLockState(force: true);
    }

    void Update()
    {
        UpdateLockState(force: false);
    }

    private void UpdateLockState(bool force)
    {
        bool locked = !GameFlags.KeycardPuzzleSolved;

        if (!force && locked == _wasLocked)
            return;

        _wasLocked = locked;

        if (_auto != null)
        {
            
            _auto.enabled = !locked;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!GameFlags.KeycardPuzzleSolved && IsPlayer(other))
        {
            
            CenterMessage.Show(lockedMessage, 2.5f);
        }
    }

    private bool IsPlayer(Collider other)
    {
        
        if (other.CompareTag("Player")) return true;

        
        return other.GetComponent<CharacterController>() != null || other.attachedRigidbody != null;
    }
}