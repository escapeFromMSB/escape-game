using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorLockByInventory : MonoBehaviour
{
    public DoorAutoOpen door;
    public string requiredItemId = "keycard_red";
    public string requiredDisplayName = "Red Keycard";
    public bool consumeItem = false;

    private bool unlocked = false;

    void Awake()
    {
        if (!door) door = GetComponent<DoorAutoOpen>();
        if (door) door.enabled = false;

        var c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (unlocked) return;
        if (!IsPlayer(other)) return;

        var inv = FindObjectOfType<PlayerInventory>();
        if (inv != null && inv.HasItem(requiredItemId))
        {
            unlocked = true;

            if (consumeItem)
                inv.RemoveItem(requiredItemId, 1);

            if (door) door.enabled = true;

            CenterMessage.Show($"Unlocked with {requiredDisplayName}.", 1.5f);
        }
        else
        {
            CenterMessage.Show($"Locked.\nNeed {requiredDisplayName}.", 1.5f);
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") || other.GetComponent<CharacterController>() != null;
    }
}