using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevatorController : MonoBehaviour
{
    public Transform platformTarget;

    [SerializeField] private float travelHeight = 4f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private bool moving = false;
    [SerializeField] private bool goingUp = false;

    void Start()
    {
        if (platformTarget == null)
        {
            platformTarget = transform; // fallback so it still moves SOMETHING
            Debug.LogWarning("[Elevator] platformTarget not set; using this.transform");
        }

        startPos = platformTarget.position;
        endPos   = startPos + new Vector3(0f, Mathf.Abs(travelHeight), 0f);

        Rigidbody rb = GetComponent<Rigidbody>();
        BoxCollider trigger = GetComponent<BoxCollider>();

        var col = GetComponent<Collider>();
        if (col == null) Debug.LogError("[Elevator] No Collider on this object. Add a BoxCollider and check Is Trigger.");
        else if (!col.isTrigger) Debug.LogWarning("[Elevator] Collider exists but Is Trigger is OFF. Turn it ON for OnTriggerEnter to fire.");

        if (speed <= 0f) Debug.LogWarning("[Elevator] Speed <= 0. Set a positive speed.");
        if (Vector3.Distance(startPos, endPos) < 0.0001f) Debug.LogWarning("[Elevator] startPos == endPos. Increase travelHeight.");
    }

    void Update()
    {
        if (!moving) return;

        Vector3 target = goingUp ? endPos : startPos;
        platformTarget.position = Vector3.MoveTowards(platformTarget.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(platformTarget.position, target) < 0.001f)
        {
            moving = false;
            Debug.Log($"[Elevator] Reached {(goingUp ? "TOP" : "BOTTOM")}");
        }
    }

    private bool IsPlayer(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag)) return other.CompareTag(playerTag);
        return other.GetComponent<CharacterController>() != null || other.attachedRigidbody != null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        // decide direction based on where we are
        float dTop = Vector3.Distance(platformTarget.position, endPos);
        float dBot = Vector3.Distance(platformTarget.position, startPos);
        goingUp = dTop > dBot;

        moving = true;
        Debug.Log("[Elevator] Triggered: moving " + (goingUp ? "UP" : "DOWN"));
    }

    // DEBUG: press Space to move without a trigger (helps test)
    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
        {
            goingUp = !goingUp;
            moving = true;
            Debug.Log("[Elevator] SPACE pressed: toggling direction to " + (goingUp ? "UP" : "DOWN"));
        }
    }
}
