using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinManager : MonoBehaviour
{
    [SerializeField] private string winSceneName = "Win";
    [SerializeField] private bool autoFindPuzzle = true;
    [SerializeField] private CollectAllPuzzle puzzle;

    void Start()
    {
        if (autoFindPuzzle && puzzle == null)
            puzzle = FindObjectOfType<CollectAllPuzzle>();

        if (puzzle != null)
            puzzle.Completed += OnPuzzleCompleted;
        else
            Debug.LogWarning("[GameWinManager] No CollectAllPuzzle found.");
    }

    private void OnDestroy()
    {
        if (puzzle != null)
            puzzle.Completed -= OnPuzzleCompleted;
    }

    private void OnPuzzleCompleted(CollectAllPuzzle p)
    {
        CenterMessage.Show("All objectives complete!\nYou escaped!", 2.0f);
        SceneManager.LoadScene(winSceneName);
    }
}