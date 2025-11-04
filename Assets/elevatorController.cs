using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    //create a platform 
    //make a start and end position 
    //think of how the elevator doors were made. but this is going up and down 
    //player collides with platform, and the platform moves to its target pos 

    public Transform platformTarget;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private bool firstFloor = false; 
    [SerializeField] private bool moving = false;
    [SerializeField] private float speed = 1f;


    [SerializeField] private float lastSeenAt = -999f;
    [SerializeField] private float openedAt   = -999f;
    [SerializeField] private string playerTag = "";

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        BoxCollider trigger = GetComponent<BoxCollider>();
        startPos = platformTarget.position;
        endPos = startPos + new Vector3(0f, 4f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
    // move elevator
    Vector3 target = moving ? endPos : startPos;
    platformTarget.position = Vector3.MoveTowards(
        platformTarget.position,
        target,
        speed * Time.deltaTime
    );

    // stop when we reach the target
        if (Vector3.Distance(platformTarget.position, target) < 0.001f)
        {
            moving = false;
        }
        
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
            moving = true;
            lastSeenAt = Time.time;
    }
    }
}
