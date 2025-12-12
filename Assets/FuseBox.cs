using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FuseBox : MonoBehaviour
{
    public string requiredItemId = "fuse";
    public string requiredDisplayName = "Fuse";
    public KeyCode interactKey = KeyCode.Q;

    private bool playerInRange = false;
    private bool installed = false;

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void Update()
    {
        if (!playerInRange || installed) return;

        if (Input.GetKeyDown(interactKey))
        {
            var inv = FindObjectOfType<PlayerInventory>();
            if (inv != null && inv.RemoveItem(requiredItemId, 1))
            {
                installed = true;
                BuildingPower.SetPower(true);
                CenterMessage.Show("Power restored!\nElevator online.", 2.0f);
            }
            else
            {
                CenterMessage.Show($"Missing {requiredDisplayName}.", 1.5f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInRange = true;
            if (!installed)
                CenterMessage.Show("Fuse Box\nPress Q to install fuse.", 1.5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other)) playerInRange = false;
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") || other.GetComponent<CharacterController>() != null;
    }
}