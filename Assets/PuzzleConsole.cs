using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PuzzleConsole : MonoBehaviour
{
    public CollectAllPuzzle puzzle;   // assign at runtime from buildScene
    public string prompt = "Press Q to start the puzzle";

    private bool playerInRange = false;

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInRange = true;
            CenterMessage.Show(prompt, 1.2f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (!playerInRange || puzzle == null) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            puzzle.Begin();
            CenterMessage.Show("Puzzle started!", 1.5f);
        }
    }

    private bool IsPlayer(Collider other)
    {
        // Tag preferred if you've set one
        if (other.CompareTag("Player")) return true;

        // Fallback heuristic
        return other.GetComponent<CharacterController>() != null || other.attachedRigidbody != null;
    }
}