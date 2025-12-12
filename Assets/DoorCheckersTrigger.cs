using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class DoorCheckersTrigger : MonoBehaviour
{
    [TextArea]
    public string message = 
        "This guy has challenged you to checkers\n" +
        "and won't let you leave until you defeat him.";

    public string checkersSceneName = "CheckersScene";

    private bool _hasTriggered = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        if (other.GetComponentInParent<PlayerController>() == null)
            return;

        _hasTriggered = true;
        SceneManager.LoadScene(checkersSceneName);
    }


    private IEnumerator LoadCheckersAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(checkersSceneName);
    }
}