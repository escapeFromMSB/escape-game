using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        
        
        Vector3 localRight = doorTarget.right;
        

        if (counterpartDoor != null)
        {
            Vector3 toCounterpart = (counterpartDoor.position - doorTarget.position).normalized;
            
            
            
            endPos = startPos + new Vector3 (0f, 0f, -1f);
        }
        else
        endPos = startPos + new Vector3 (0f, 0f, 1f);
    }

void Update()
{
    
    
    isOpen = false;

    
    if (Time.time - lastSeenAt <= exitGraceSeconds)
        isOpen = true;

    
    if (Time.time - openedAt <= minOpenSeconds)
        isOpen = true;

    
    if (!opening && isOpen)
        openedAt = Time.time;

    
    opening = isOpen;

    
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
        lastSeenAt = Time.time; 
    }
}
}



