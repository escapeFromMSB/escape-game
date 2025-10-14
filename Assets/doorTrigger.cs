using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code currently disables the solid collider so the player can walk through the door 
// TODO: make the door rotate 90 degrees on trigger 
// TODO: implement a key 

public class doorTrigger : MonoBehaviour
{
    // quarternion: calculates and stores rotations 
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool opening = false; 

    public string doorName;
    public float openSpeed = 2f;

    void Awake()
    {
        // Ensure a Rigidbody exists so trigger events work
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            Debug.Log($"{doorName}: Added kinematic Rigidbody to support trigger events.");
        }

        // get the current rotation of the door (closed) and store it. 
        closedRotation = transform.rotation;
        Debug.Log($"{doorName}: Closed rotation stored = {closedRotation.eulerAngles}");

        // apply the open rotation angle relative to the closed rotation angle and store it.
        openRotation = closedRotation * Quaternion.Euler(0f, 90f, 0f);
        Debug.Log($"{doorName}: Open rotation calculated = {openRotation.eulerAngles}");
    }

    private void OnTriggerEnter(Collider component)
    {
        if (component.CompareTag("Player"))
        {
            Debug.Log($"{doorName}: Player entered trigger ({component.name})");

            // TODO: check if player has key, then open
            // if (key)
            opening = true;
            Debug.Log($"{doorName}: Opening sequence started.");
        }
        else
        {
            Debug.Log($"{doorName}: Something else entered trigger ({component.name})");
        }
    }

    // use update for a smooth transition 
    void Update()
    {
        if (opening && !isOpen)
        {
            // Smooth rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
            Debug.Log($"{doorName}: Rotating... Current = {transform.rotation.eulerAngles}");

            // Snap and finish
            if (Quaternion.Angle(transform.rotation, openRotation) < 0.5f)
            {
                transform.rotation = openRotation;
                isOpen = true;
                opening = false;
                Debug.Log($"{doorName}: Door fully opened at {openRotation.eulerAngles}");

                // Disable the solid collider so you can walk through
                var solid = GetComponent<BoxCollider>();
                if (solid != null && !solid.isTrigger)
                {
                    solid.enabled = false;
                    Debug.Log($"{doorName}: Solid collider disabled so player can pass through.");
                }
            }
        }
    }
}
