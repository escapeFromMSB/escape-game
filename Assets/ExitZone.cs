using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ExitZone : MonoBehaviour
{
    public string winSceneName = "Win";

    void Reset()
    {
        var c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}