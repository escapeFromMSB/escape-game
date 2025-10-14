using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSetup : MonoBehaviour

{
    void Awake()
    {
        // Ignore everything by default
        for (int i = 0; i < 32; i++)
        {
            Physics.IgnoreLayerCollision(Layers.DoorTrigger, i, true);
        }

        // Allow only DoorTrigger <-> Player
        Physics.IgnoreLayerCollision(Layers.DoorTrigger, Layers.Player, false);

    }
}
