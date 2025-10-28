using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// these doors will automatically open and close like elevator doors. they will try to close and if the user is there, they will reopen 
public class ElevatorDoor : MonoBehaviour{

public Transform doorTarget;
[SerializeField] private Vector3 startPos;
[SerializeField] private Vector3 endPos;
[SerializeField] private bool isOpen = false; 
[SerializeField] private bool opening = false; 
[SerializeField] private int overlapCount;
[SerializeField] private float speed = 1f;


[SerializeField] private float lastSeenAt = -999f;
[SerializeField] private float openedAt   = -999f;
[SerializeField] private string playerTag = "";

public Transform counterpartDoor;


// ----- Anti-flap (hysteresis) -----
    [Header("Hysteresis")]
    [Tooltip("Stay open this long after the player leaves the trigger.")]
    [SerializeField] private float exitGraceSeconds = 5f;
    [Tooltip("Once opened, don't allow closing earlier than this.")]
    [SerializeField] private float minOpenSeconds   = 5f;



 public void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        BoxCollider trigger = GetComponent<BoxCollider>();
        startPos = doorTarget.position;

        // Choose slide axis based on the door's local right vector.
        // If we have a counterpart, push away from it; otherwise slide +right.
        Vector3 localRight = doorTarget.right;
        

        if (counterpartDoor != null)
        {
            Vector3 toCounterpart = (counterpartDoor.position - doorTarget.position).normalized;
            // If counterpart is to our right (>0), we slide to the LEFT (negative right),
            // so panels part away from the center line.
            
            endPos = startPos + new Vector3 (0f, 0f, -1f);
        }
        else
        endPos = startPos + new Vector3 (0f, 0f, 1f);
    }

void Update()
{
    // --- decide if we should be open ---
    // Reset every frame
    isOpen = false;

    // Player recently seen (grace window)
    if (Time.time - lastSeenAt <= exitGraceSeconds)
        isOpen = true;

    // Still within minimum open duration
    if (Time.time - openedAt <= minOpenSeconds)
        isOpen = true;

    // Transition detection: just opened
    if (!opening && isOpen)
        openedAt = Time.time;

    // Sync opening flag 
    opening = isOpen;

    // --- move door ---
    Vector3 target = isOpen ? endPos : startPos;
    doorTarget.position = Vector3.MoveTowards(
        doorTarget.position,
        target,
        speed * Time.deltaTime
    );

}


    private bool IsPlayer(Collider other)
    {
        if (!string.IsNullOrEmpty(playerTag))
            return other.CompareTag(playerTag);

        // Fallback: accept anything with a CharacterController
        return other.GetComponent<CharacterController>() != null;
    }

void OnTriggerEnter(Collider other){
	if (IsPlayer(other)){
    lastSeenAt = Time.time;
}
}
void OnTriggerStay(Collider other){
    if (IsPlayer(other)){
        
        lastSeenAt = Time.time;
    }
}
void OnTriggerExit(Collider other)
{
    if (IsPlayer(other)){
        lastSeenAt = Time.time; // keep "seen" fresh while grazing the edge
    }
}
}


//notes for tomorrow: enter your last seen at stuff and player and grace seconds. most of this will be used in update. 
