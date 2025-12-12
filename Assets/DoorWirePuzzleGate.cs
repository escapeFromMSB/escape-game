using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class DoorWirePuzzleGate : MonoBehaviour
{
    [Header("Puzzle")]
    public string puzzleSceneName = "WirePuzzleScene";
    public string returnSceneName = "MainGame";

    [Header("Messaging")]
    public string lockedPrompt = "Door locked.\nFix the wiring to open it.";
    public float promptSeconds = 1.2f;

    [Header("Behavior")]
    public bool reenableAutoOpenAfterSolve = true;

    private DoorAutoOpen autoOpen;

    void Awake()
    {
        
        
        autoOpen = GetComponent<DoorAutoOpen>();

        
        if (autoOpen != null)
            autoOpen.enabled = false;

        
        var bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;

        
        var rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Start()
    {
        SyncSolvedState();
    }

    private void SyncSolvedState()
    {
        if (!GameFlags.FirstRoomWiresSolved) return;

        if (autoOpen != null && reenableAutoOpenAfterSolve)
            autoOpen.enabled = true;

        
        enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        if (GameFlags.FirstRoomWiresSolved)
        {
            SyncSolvedState();
            return;
        }

        
        CenterMessage.Show(lockedPrompt, promptSeconds);

        
        if (!string.IsNullOrWhiteSpace(puzzleSceneName))
            SceneManager.LoadScene(puzzleSceneName, LoadSceneMode.Single);
        else
            Debug.LogError("DoorWirePuzzleGate: puzzleSceneName is empty.");
    }

    private bool IsPlayer(Collider other)
    {
        if (other.CompareTag("Player")) return true;
        return other.GetComponent<CharacterController>() != null;
    }
}
