using UnityEngine;

public class PlayerBootstrap : MonoBehaviour
{
    [Header("Floor reference")]
    public GameObject firstFloor;   

    [Header("Spawn offsets (XZ)")]
    public Vector2 initialSpawnOffset = new Vector2(-24f, -25f);
    public Vector2 afterWiresSpawnOffset = new Vector2(-27.5f, -19.5f);

    void Start()
    {
        
        
        if (FindObjectOfType<PlayerController>() != null)
            return;

        if (firstFloor == null)
        {
            Debug.LogError("PlayerBootstrap: firstFloor is not assigned.");
            return;
        }

        
        Vector2 off2D = GameFlags.FirstRoomWiresSolved
            ? afterWiresSpawnOffset
            : initialSpawnOffset;

        SpawnPlayer(firstFloor, off2D);
    }

    void SpawnPlayer(GameObject floor, Vector2 offsetXZ)
    {
        var rend = floor.GetComponent<Renderer>();
        float topY = rend ? rend.bounds.max.y : floor.transform.position.y;

        
        Vector3 spawn = new Vector3(offsetXZ.x, topY + 3.1f, offsetXZ.y);
        CreateDefaultPlayer(spawn);
    }

    GameObject CreateDefaultPlayer(Vector3 spawnPos)
    {
        GameObject playerRoot = new GameObject("Player");
        playerRoot.transform.position = spawnPos;

        
        var cc = playerRoot.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.radius = 0.4f;
        cc.center = new Vector3(0f, 1f, 0f);

        
        Camera existing = Camera.main;
        GameObject camGO;
        if (existing != null)
        {
            
            camGO = existing.gameObject;
            camGO.tag = "MainCamera";
            camGO.transform.SetParent(playerRoot.transform, worldPositionStays: false);
            camGO.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            camGO.transform.localRotation = Quaternion.identity;
        }
        else
        {
            camGO = new GameObject("PlayerCamera");
            camGO.tag = "MainCamera";
            camGO.AddComponent<Camera>();
            camGO.transform.SetParent(playerRoot.transform, worldPositionStays: false);
            camGO.transform.localPosition = new Vector3(0f, 1.6f, 0f);
        }

        
        camGO.AddComponent<CrosshairUI>();

        var controller = playerRoot.AddComponent<PlayerController>();
        controller.CameraPivot = camGO.transform;

        var interactor = playerRoot.AddComponent<PlayerInteractor>();
        interactor.viewCamera = camGO.GetComponent<Camera>();

        return playerRoot;
    }
}
