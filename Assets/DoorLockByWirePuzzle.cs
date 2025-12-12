using UnityEngine;

public class DoorLockByWirePuzzle : MonoBehaviour
{
    public DoorAutoOpen door;
    public string lockedMessage = "Door locked.\nFix the wiring panel.";

    private bool unlocked = false;

    void Awake()
    {
        if (door == null)
            door = GetComponent<DoorAutoOpen>();

        if (door != null)
            door.enabled = false;
    }

    void Update()
    {
        if (unlocked) return;

        if (GameFlags.FirstRoomWiresSolved)
        {
            unlocked = true;
            if (door != null)
                door.enabled = true;

            CenterMessage.Show("Door unlocked!", 1.25f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (unlocked) return;

        if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
        {
            CenterMessage.Show(lockedMessage, 1.25f);
        }
    }
}