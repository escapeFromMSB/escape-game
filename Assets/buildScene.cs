using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildScene : MonoBehaviour
{
    public enum SideOne { left, right, front, back, above, below }
    public enum SideTwo { left, right, front, back, above, below }
    private static bool _built = false;

    void Start()
    {
        if (_built) return;
        _built = true;

        float sideOneOffset = 0f;
        float sideTwoOffset = 0f;

        //--- create the  first floor (plane) ---
        // createPrimitive creates a 3D object 
        GameObject firstFloor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        firstFloor.name = "firstFloor";

        // set the position
        // vector3 means 3 coordinates
        firstFloor.transform.position = new Vector3 (0,0,0);

        // set scale to normal scale 
        // f means float. needs to be float and not double due to performance because float is faster
        // *** in unity, 1 unit = 1 meter. the scale (1,1,1) is 10x10 by default (for a plane it isx and z axis). ***
        firstFloor.transform.localScale = new Vector3 (5f,1f,5f); //plane is 50x50

        // --- create wallOne (cube) ---
        GameObject wallOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallOne.name = "wallOne";
        wallOne.transform.localScale = new Vector3 (1f,3f,6f);

        // place wallOne on top of firstFloor 
        // calculate object positions from the plane’s current size and center, instead of hard-coding numbers.
        // this way we can update the plane size if needed without messing everyting else up
        PlaceObjectOnPlane(firstFloor, wallOne, SideOne.left, SideTwo.front, 0f, 0f);

        // a renderer is a unity component that draws the object on the screen
        // get the renderer components for each object
        
        // --- create DoorOne (cube) ---
        GameObject doorOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doorOne.name = "doorOne";
        doorOne.transform.localScale = new Vector3 (1f,2f,0.1f);
        PlaceObjectOnPlane(firstFloor, doorOne, SideOne.left, SideTwo.front, 1f, 0f);
        addGlassMaterial(doorOne);

        // --- create DoorTwo (cube) ---
        GameObject doorTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doorTwo.name = "doorTwo";
        doorTwo.transform.localScale = new Vector3 (1f,2f,0.1f);
        PlaceObjectOnPlane(firstFloor, doorTwo, SideOne.left, SideTwo.front, 2f, 0f);
        addGlassMaterial(doorTwo);

        GameObject wallThree = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallThree.name = "wallThree";
        wallThree.transform.localScale = new Vector3 (2f,3f,1f);
        PlaceObject(wallOne, wallThree, SideOne.left, SideTwo.front, 4f, 4f);

        addMaterial(wallOne, Color.yellow);
        


        // --- Aim the Main Camera so you can see everything ---
        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0f, 25f, -35f);
            cam.transform.LookAt(new Vector3(0f, 0f, 0f));
        }
        else
        {
            Debug.LogWarning("No Main Camera found. Create one (GameObject > Camera).");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// --- ALIGNMENT FUNCTIONS ---

void PlaceObject(GameObject a, GameObject b, SideOne sideOne, SideTwo sideTwo,
                 float edgeOneOffset, float edgeTwoOffset)
{
    // renderers for both objects
    Renderer anchorRenderer = a.GetComponent<Renderer>();
    Renderer objectRenderer = b.GetComponent<Renderer>();
    // use renderer to get the objects bounds and half of its size 
    // components of object being aligned to 

    // half-sizes of objects
    Vector3 halfA = anchorRenderer.bounds.extents;   // anchor half-size
    Vector3 halfB = objectRenderer.bounds.extents;   // object half-size

    // position of object that is being aligned to (anchor)
    Vector3 posA = a.transform.position;
    // temp position 
    Vector3 pos = posA;

    // top/bottom of anchor
    float topA = anchorRenderer.bounds.max.y;
    float bottomA = anchorRenderer.bounds.min.y;

    // --- sideOne ---
    switch (sideOne)
    {
        case SideOne.right: pos.x = posA.x - (halfA.x - halfB.x) + edgeOneOffset; break;
        case SideOne.left:  pos.x = posA.x + (halfA.x - halfB.x) - edgeOneOffset; break;
        case SideOne.front: pos.z = posA.z - (halfA.z - halfB.z) + edgeOneOffset; break;
        case SideOne.back:  pos.z = posA.z + (halfA.z - halfB.z) - edgeOneOffset; break;
        case SideOne.above: pos.y = topA + halfB.y + edgeOneOffset; break;
        case SideOne.below: pos.y = bottomA - halfB.y - edgeOneOffset; break;
    }

    // --- sideTwo ---
    switch (sideTwo)
    {
        case SideTwo.right: pos.x = posA.x - (halfA.x - halfB.x) + edgeTwoOffset; break;
        case SideTwo.left:  pos.x = posA.x + (halfA.x - halfB.x) - edgeTwoOffset; break;
        case SideTwo.front: pos.z = posA.z - (halfA.z - halfB.z) + edgeTwoOffset; break;
        case SideTwo.back:  pos.z = posA.z + (halfA.z - halfB.z) - edgeTwoOffset; break;
        case SideTwo.above: pos.y = topA + halfB.y + edgeTwoOffset; break;
        case SideTwo.below: pos.y = bottomA - halfB.y - edgeTwoOffset; break;
    }

    // move b into place
    b.transform.position = pos;
}

void PlaceObjectOnPlane(GameObject a, GameObject b, SideOne sideOne, SideTwo sideTwo,
                        float edgeOneOffset, float edgeTwoOffset)
{
    // renderers for both objects
    Renderer anchorRenderer = a.GetComponent<Renderer>();
    Renderer objectRenderer = b.GetComponent<Renderer>();

    // half-sizes of objects
    Vector3 halfA = anchorRenderer.bounds.extents;   // anchor half-size
    Vector3 halfB = objectRenderer.bounds.extents;   // object half-size

    // anchor position
    Vector3 posA = a.transform.position;
    Vector3 pos = posA;

    // top/bottom of anchor
    float topA = anchorRenderer.bounds.max.y;
    float bottomA = anchorRenderer.bounds.min.y;

    // --- sideOne ---
    switch (sideOne)
    {
        case SideOne.right:
            pos.x = posA.x - (halfA.x - halfB.x) + edgeOneOffset; // keep b inside the right edge
            pos.y = topA + halfB.y;               // rest on top
            break;

        case SideOne.left:
            pos.x = posA.x + (halfA.x - halfB.x) - edgeOneOffset;
            pos.y = topA + halfB.y;
            break;

        case SideOne.front:
            pos.z = posA.z - (halfA.z - halfB.z) + edgeOneOffset;
            pos.y = topA + halfB.y;
            break;

        case SideOne.back:
            pos.z = posA.z + (halfA.z - halfB.z) - edgeOneOffset;
            pos.y = topA + halfB.y;
            break;

        case SideOne.above:
            pos.y = topA + halfB.y + edgeOneOffset;
            break;

        case SideOne.below:
            pos.y = bottomA - halfB.y - edgeOneOffset;
            break;
    }

    // --- sideTwo ---
    switch (sideTwo)
    {
        case SideTwo.right:
            pos.x = posA.x - (halfA.x - halfB.x) + edgeTwoOffset; // keep b inside the right edge
            pos.y = topA + halfB.y;               // rest on top
            break;

        case SideTwo.left:
            pos.x = posA.x + (halfA.x - halfB.x) - edgeTwoOffset;
            pos.y = topA + halfB.y;
            break;

        case SideTwo.front:
            pos.z = posA.z - (halfA.z - halfB.z) + edgeTwoOffset;
            pos.y = topA + halfB.y;
            break;

        case SideTwo.back:
            pos.z = posA.z + (halfA.z - halfB.z) - edgeTwoOffset;
            pos.y = topA + halfB.y;
            break;

        case SideTwo.above:
            pos.y = topA + halfB.y + edgeTwoOffset;
            break;

        case SideTwo.below:
            pos.y = bottomA - halfB.y - edgeTwoOffset;
            break;
    }

    // clamp inside X/Z so b never hangs off the anchor
    pos.x = Mathf.Clamp(pos.x, posA.x - halfA.x + halfB.x, posA.x + halfA.x - halfB.x);
    pos.z = Mathf.Clamp(pos.z, posA.z - halfA.z + halfB.z, posA.z + halfA.z - halfB.z);

    // move b into place
    b.transform.position = pos;
}

// --- FUNCTION TO ADD MATERIAL TO OBJECT
void addMaterial(GameObject obj, Color objColor){
    // grab its renderer
    Renderer rend = obj.GetComponent<Renderer>();

    //create a new material
    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));

    //set material color and assign it to the object
    mat.color = objColor;
    rend.material = mat;
}

void addGlassMaterial(GameObject obj){
    Renderer rend = obj.GetComponent<Renderer>();

    // Create a URP/Lit material
    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));

    // Light blue tint, partly see-through
    mat.SetColor("_BaseColor", new Color(0.9f, 0.9f, 1f, 0.1f)); 

    // Tell Unity it’s transparent
    mat.SetFloat("_Surface", 1f);  
    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

    // Make it shiny like glass
    mat.SetFloat("_Smoothness", 1f);
    mat.SetFloat("_Metallic", 1f);

    // Assign to object
    rend.material = mat;
}
}