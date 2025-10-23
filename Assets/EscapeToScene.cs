using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "MainGame"; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!string.IsNullOrWhiteSpace(targetSceneName))
                SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
            else
                Debug.LogError("EscapeToScene: targetSceneName is empty. Set it in the Inspector.");
        }
    }
}