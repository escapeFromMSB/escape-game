using UnityEngine;

public class WirePuzzleBoot : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;
    }
}