using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created for physics purposes. without them, every object (such as the wall) is reacting with the trigger
public class Layers : MonoBehaviour
{
    //player layer 
    public const int Player = 8;      
    //door trigger layer 
    public const int DoorTrigger = 9; 
}