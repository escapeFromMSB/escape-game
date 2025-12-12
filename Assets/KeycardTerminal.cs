using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeycardTerminal : MonoBehaviour
{
    [Header("Setup")]
    public KeycardSequencePuzzle puzzle;
    [Tooltip("Item ID this terminal accepts (e.g. keycard_red).")]
    public string itemId = "keycard_red";
    [Tooltip("Name shown in prompts (e.g. Red Keycard).")]
    public string itemLabel = "Red Keycard";

    [Header("Prompts")]
    [Tooltip("Prompt shown when player is in range.")]
    public string promptFormat = "Press Q to insert {0}";

    private bool _playerInRange = false;

    void Reset()
    {
        
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        _playerInRange = true;

        CenterMessage.Show(string.Format(promptFormat, itemLabel), 1.5f);

        if (puzzle != null)
        {
            puzzle.ShowRiddle();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        _playerInRange = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!_playerInRange || !IsPlayer(other)) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            var inventory = FindObjectOfType<PlayerInventory>();
            if (!inventory)
            {
                CenterMessage.Show("No inventory found.", 1.5f);
                return;
            }

            if (!inventory.HasItem(itemId, 1))
            {
                CenterMessage.Show($"You don't have {itemLabel}.", 1.8f);
                return;
            }

            if (puzzle != null)
            {
                puzzle.TryInsertCard(itemId, itemLabel);
            }
            else
            {
                CenterMessage.Show("This terminal is not connected to a puzzle.", 1.5f);
            }
        }
    }

    private bool IsPlayer(Collider other)
    {
        if (other.CompareTag("Player")) return true;

        
        return other.GetComponent<CharacterController>() != null;
    }
}
