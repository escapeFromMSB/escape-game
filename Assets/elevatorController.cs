using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevatorController : MonoBehaviour
{
    public Transform platformTarget;

    [SerializeField] private float speed = 1.5f;
    [SerializeField] private string playerTag = "";

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private bool moving = false;
    [SerializeField] private bool goingUp = false;

    void Start()
    {
        if (platformTarget == null)
        {
            platformTarget = transform;
            Debug.LogWarning("[Elevator] platformTarget not set; using this.transform");
        }

        startPos = platformTarget.position;
        endPos = startPos + new Vector3(0f, -3f, 0f);

        var col = GetComponent<Collider>();
        if (col == null)
            Debug.LogError("[Elevator] No Collider on this object. Add a BoxCollider and check Is Trigger.");
        else if (!col.isTrigger)
            Debug.LogWarning("[Elevator] Collider exists but Is Trigger is OFF. Turn it ON for OnTriggerEnter to fire.");

        if (speed <= 0f)
            Debug.LogWarning("[Elevator] Speed <= 0. Set a positive speed.");

        Debug.Log($"[Elevator] Start initialized. StartPos={startPos}, EndPos={endPos}");
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

        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        // Fallback: accept anything with a CharacterController
        return other.GetComponent<CharacterController>() != null;

        bool hasCC = other.GetComponent<CharacterController>() != null;
        bool hasRB = other.attachedRigidbody != null;
        Debug.Log($"[Elevator] Checking collider '{other.name}': CC={hasCC}, RB={hasRB}");
        return hasCC || hasRB;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Elevator] OnTriggerEnter called by '{other.name}'");

        if (!IsPlayer(other))
        {
            Debug.Log("[Elevator] Collider was NOT a player, ignoring.");
            return;
        }

        float dTop = Vector3.Distance(platformTarget.position, endPos);
        float dBot = Vector3.Distance(platformTarget.position, startPos);
        goingUp = dTop > dBot;

        moving = true;
        Debug.Log($"[Elevator] Triggered by '{other.name}' → moving {(goingUp ? "UP" : "DOWN")}");
        if (IsAtBottom())
        {
            Debug.Log("[Elevator] Currently on FIRST floor.");
        }
        else if (IsAtTop())
        {
            Debug.Log("[Elevator] Currently on SECOND floor.");
        }


    }


}
