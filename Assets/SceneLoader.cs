using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Names (must match Build Settings)")]
    [SerializeField] private string mainMenu = "MenuScene";
    [SerializeField] private string game     = "MainGame";
    [SerializeField] private string howTo    = "HowToPlay";
    [SerializeField] private string credits  = "Credits";

    // Pre-wired helpers (no typing strings in OnClick)
    public void LoadMainMenu() => LoadByName(mainMenu);
    public void LoadGame()     => LoadByName(game);
    public void LoadHowTo()    => LoadByName(howTo);
    public void LoadCredits()  => LoadByName(credits);

    // Generic loader (use if you want to type the name in OnClick)
    public void LoadByName(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("SceneLoader: scene name is empty.");
            return;
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}