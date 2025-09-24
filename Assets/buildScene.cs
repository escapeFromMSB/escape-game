using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildScene : MonoBehaviour
{
    public enum SideOne { left, right, front, back, above, below }
    public enum SideTwo { left, right, front, back, above, below }
    public enum SideThree { left, right, front, back, above, below, nothing }
    private static bool _built = false;

    void Start()
    {
        if (_built) return;
        _built = true;

        float sideOneOffset = 0f;
        float sideTwoOffset = 0f;
        float edgeThreeOffset = 0f;

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

        //another first floor plane 
        GameObject firstFloorPlaneTwo = GameObject.CreatePrimitive(PrimitiveType.Plane);
        firstFloorPlaneTwo.name = "firstFloorPlaneTwo";
        firstFloorPlaneTwo.transform.position = new Vector3 (-50,0,0);
        firstFloorPlaneTwo.transform.localScale = new Vector3 (5f,1f,5f);

        //cylinder
        GameObject cylinderOne = BuildCylinder(firstFloor, "coneOne", new Vector3(1.25f, 1.5f, 1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.75f, 8f, 0f);
        GameObject cylinderTwo = BuildCylinder(firstFloor, "cylinderTwo", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -0.75f, 9f, 0f);
        //try rotating this

        // --- WALLS ---
        GameObject wallOne = BuildWall(firstFloor, "wallOne", new Vector3(1f, 3f, 9f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 0f, 0f);
        GameObject wallTwo = BuildWall(firstFloor, "wallTwo", new Vector3 (0.5f, 3f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, -5f, 0f, 0f);
        GameObject wallThree = BuildWall(firstFloor, "wallThree", new Vector3 (1f, 3f, 3f), SideOne.left,  SideTwo.front, SideThree.nothing, -4f, 0f, 0f);
        GameObject wallOneBesideDoorTwo = BuildWall(firstFloor, "wallOneBesideDoorTwo", new Vector3 (0.5f, 3f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1f, 0f, 0f);
        GameObject wallTwoBesideDoorTwo = BuildWall(firstFloor, "wallTwoBesideDoorTwo", new Vector3 (0.5f, 3f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -3.5f, 0f, 0f);
        GameObject wallFour = BuildWall(firstFloor, "wallFour", new Vector3(1f, 3f, 6f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 0f, 0f);
        GameObject wallFive = BuildWall(firstFloor, "wallFive", new Vector3(8f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, -5f, 0f, 2.5f);
        GameObject wallSix = BuildWall(firstFloor, "wallSix", new Vector3(8f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, -5f, 0f, 0f);
        GameObject wallSeven = BuildWall(firstFloor, "wallSeven", new Vector3 (1.5f, 3f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, -11.75f, 0f, 0f);
        GameObject wallEight = BuildWall(firstFloorPlaneTwo, "wallEight", new Vector3 (1.5f, 3f, 1f), SideOne.right,  SideTwo.front, SideThree.nothing, 0.85f, 9f, 0f);
        GameObject wallNine = BuildWall(firstFloor, "wallNine", new Vector3(1f, 3f, 13f), SideOne.left,  SideTwo.front, SideThree.nothing, -13.25f, 0f, 0f);
        GameObject wallTen = BuildWall(firstFloor, "wallTen", new Vector3(10f, 3f, 1f), SideOne.left,  SideTwo.front, SideThree.nothing, -3.25f, 13f, 0f);
        GameObject wallAboveCylinder = BuildWall(firstFloor, "wallAboveCylinder", new Vector3(0.5f, 0.5f, 13f), SideOne.left,  SideTwo.front, SideThree.above, -4f, 0f, 2.5f);
        // Rotate 90° around Y relative to current rotation
        wallEight.transform.Rotate(0f, 45f, 0f);

    
    

        // --- DOORS ---
        BuildDoor(firstFloor, "doorOne",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 0f, 0f);
        BuildDoor(firstFloor, "doorTwo",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 0f, 0f);
        BuildDoor(firstFloor, "doorThree",new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 2.9f, 0f);
        BuildDoor(firstFloor, "doorFour", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 2.9f, 0f);
        // --- glass pannels besides doors ---
        BuildGlassPannel(firstFloor, "glassPannelOne", new Vector3 (0.5f, 2f, 0.1f), SideOne.left, SideTwo.front, SideThree.nothing, -1f, 2.9f, 0f);
        BuildGlassPannel(firstFloor, "glassPannelTwo", new Vector3 (0.5f, 2f, 0.1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.5f, 2.9f, 0f); 

        // --- windows ---
        BuildWindow(wallFive, "windowOne", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -0.5f, -0.05f, 0f);
        BuildWindow(wallSix, "windowTwo", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -0.5f, -0.05f, 0f);
        BuildWindow(wallFive, "windowThree", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -1.75f, -0.05f, 0f);
        BuildWindow(wallSix, "windowFour", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -1.75f, -0.05f, 0f);
        BuildWindow(wallFive, "windowFive", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -3f, -0.05f, 0f);
        BuildWindow(wallSix, "windowSix", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -3f, -0.05f, 0f);
        BuildWindow(wallFive, "windowSeven", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -4.25f, -0.05f, 0f);
        BuildWindow(wallSix, "windowEight", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -4.25f, -0.05f, 0f);
        BuildWindow(wallFive, "windowNine", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -5.5f, -0.05f, 0f);
        BuildWindow(wallSix, "windowTen", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -5.5f, -0.05f, 0f);
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

void PlaceObject(GameObject a, GameObject b, SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                 float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset)
{
    // renderers for both objects
    // a renderer is a unity component that draws the object on the screen
    // use renderer to get the objects bounds and half of its size 
    // components of object being aligned to 
    Renderer anchorRenderer = a.GetComponent<Renderer>();
    Renderer objectRenderer = b.GetComponent<Renderer>();


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
        case SideOne.right: pos.x = posA.x + (halfA.x - halfB.x) + edgeOneOffset; break;
        case SideOne.left:  pos.x = posA.x - (halfA.x - halfB.x) - edgeOneOffset; break;
        case SideOne.front: pos.z = posA.z + (halfA.z - halfB.z) + edgeOneOffset; break;
        case SideOne.back:  pos.z = posA.z - (halfA.z - halfB.z) - edgeOneOffset; break;
        case SideOne.above: pos.y = topA + halfB.y + edgeOneOffset; break;
        case SideOne.below: pos.y = bottomA - halfB.y - edgeOneOffset; break;
    }

    // --- sideTwo ---
    switch (sideTwo)
    {
        case SideTwo.right: pos.x = posA.x + (halfA.x - halfB.x) + edgeTwoOffset; break;
        case SideTwo.left:  pos.x = posA.x - (halfA.x - halfB.x) - edgeTwoOffset; break;
        case SideTwo.front: pos.z = posA.z + (halfA.z - halfB.z) + edgeTwoOffset; break;
        case SideTwo.back:  pos.z = posA.z - (halfA.z - halfB.z) - edgeTwoOffset; break;
        case SideTwo.above: pos.y = topA + halfB.y + edgeTwoOffset; break;
        case SideTwo.below: pos.y = bottomA - halfB.y - edgeTwoOffset; break;
    }

    // --- sideThree ---
    switch (sideThree)
    {
        case SideThree.right: pos.x = posA.x + (halfA.x - halfB.x) + edgeThreeOffset; break;
        case SideThree.left:  pos.x = posA.x - (halfA.x - halfB.x) - edgeThreeOffset; break;
        case SideThree.front: pos.z = posA.z + (halfA.z - halfB.z) + edgeThreeOffset; break;
        case SideThree.back:  pos.z = posA.z - (halfA.z - halfB.z) - edgeThreeOffset; break;
        case SideThree.above: pos.y = topA + halfB.y + edgeThreeOffset; break;
        case SideThree.below: pos.y = bottomA - halfB.y - edgeThreeOffset; break;
        case SideThree.nothing: break;
    }

    // move b into place
    b.transform.position = pos;
}

void PlaceObjectOnPlane(GameObject a, GameObject b, SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                        float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset)
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
            pos.x = posA.x + (halfA.x - halfB.x) + edgeOneOffset; // keep b inside the right edge
            pos.y = topA + halfB.y;               // rest on top
            break;

        case SideOne.left:
            pos.x = posA.x - (halfA.x - halfB.x) - edgeOneOffset;
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
            pos.x = posA.x + (halfA.x - halfB.x) + edgeTwoOffset; // keep b inside the right edge
            pos.y = topA + halfB.y;               // rest on top
            break;

        case SideTwo.left:
            pos.x = posA.x - (halfA.x - halfB.x) - edgeTwoOffset;
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

    // --- sideThree ---
    switch (sideThree)
    {
        case SideThree.right:
            pos.x = posA.x + (halfA.x - halfB.x) + edgeThreeOffset;
            break;

        case SideThree.left:
            pos.x = posA.x - (halfA.x - halfB.x) - edgeThreeOffset;
            break;

        case SideThree.front:
            pos.z = posA.z - (halfA.z - halfB.z) + edgeThreeOffset;
            break;

        case SideThree.back:
            pos.z = posA.z + (halfA.z - halfB.z) - edgeThreeOffset;
            break;

        case SideThree.above:
            pos.y = topA + halfB.y + edgeThreeOffset;
            break;

        case SideThree.below:
            pos.y = bottomA - halfB.y - edgeThreeOffset;
            break;
        case SideThree.nothing:
            break;
    }

    // clamp inside X/Z so b never hangs off the anchor
    // pos.x = Mathf.Clamp(pos.x, posA.x - halfA.x + halfB.x, posA.x + halfA.x - halfB.x);
    // pos.z = Mathf.Clamp(pos.z, posA.z - halfA.z + halfB.z, posA.z + halfA.z - halfB.z);

    // move b into place
    b.transform.position = pos;
}

// --- FUNCTIONS TO ADD MATERIAL TO OBJECT
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


// BUILD FUNCTIONS 
void BuildDoor(GameObject floor, string doorName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset){
    // --- create the door (main cube) ---
    GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
    door.name = doorName;
    door.transform.localScale = scale;

    // place on the plane
    PlaceObjectOnPlane(floor, door, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);

    // make it glass
    addGlassMaterial(door);

    // --- add door pannels ---
    GameObject pannelOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelOne.name = doorName + "PannelOne";
    pannelOne.transform.localScale = new Vector3(0.1f, 2f, 0.05f);
    //make pannel a child of door 
    pannelOne.transform.SetParent(door.transform);
    PlaceObject(door, pannelOne, SideOne.left, SideTwo.back, SideThree.nothing, 0f, -0.1f, 0f);

    GameObject pannelTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelTwo.name = doorName + "PannelTwo";
    pannelTwo.transform.localScale = new Vector3(0.1f, 2f, 0.05f);
    pannelTwo.transform.SetParent(door.transform);
    PlaceObject(door, pannelTwo, SideOne.right, SideTwo.back, SideThree.nothing, 0f, -0.1f, 0f);

    GameObject pannelThree = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelThree.name = doorName + "PannelThree";
    pannelThree.transform.localScale = new Vector3(1f, 0.1f, 0.05f);
    pannelThree.transform.SetParent(door.transform);
    PlaceObject(door, pannelThree, SideOne.above, SideTwo.back, SideThree.nothing, -0.1f, -0.1f, 0f);

    GameObject pannelFour = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelFour.name = doorName + "PannelFour";
    pannelFour.transform.localScale = new Vector3(1f, 0.1f, 0.05f);
    pannelFour.transform.SetParent(door.transform);
    PlaceObject(door, pannelFour, SideOne.below, SideTwo.back, SideThree.nothing, -0.1f, -0.1f, 0f);
}

void BuildGlassPannel(GameObject floor, string pannelName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree, 
                        float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset ){

    GameObject glassPannel = GameObject.CreatePrimitive(PrimitiveType.Cube);
    glassPannel.name = pannelName;
    glassPannel.transform.localScale = scale;
    PlaceObjectOnPlane(floor, glassPannel, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);
    addGlassMaterial(glassPannel);

}

GameObject BuildWall(GameObject floor, string wallName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset){
    // calculate object positions from the plane’s current size and center, instead of hard-coding numbers.
    // this way we can update the plane size if needed without messing everyting else up
    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    wall.name = wallName;
    wall.transform.localScale = scale;
    PlaceObjectOnPlane(floor, wall, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);
    return wall;
}

GameObject BuildCylinder(GameObject floor, string cylinderName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree, 
                        float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset){
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.name = cylinderName;
        cylinder.transform.localScale = scale;
        PlaceObjectOnPlane(floor, cylinder, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);
        return cylinder;
    }


void BuildWindow(GameObject anchorObj, string windowName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree, 
                float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset){
    GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
    window.name = windowName;
    window.transform.localScale = scale;
    PlaceObject(anchorObj, window, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);
    addGlassMaterial(window);

    // --- add window pannels ---
    GameObject windowPannelOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
    windowPannelOne.name = windowName + "PannelOne";
    windowPannelOne.transform.localScale = new Vector3(0.1f, 1f, 0.05f);
    //make pannel a child of window 
    windowPannelOne.transform.SetParent(window.transform);
    PlaceObject(window, windowPannelOne, SideOne.left, SideTwo.back, SideThree.nothing, 0f, -0.1f, 0f);

    GameObject windowPannelTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
    windowPannelTwo.name = windowName + "PannelTwo";
    windowPannelTwo.transform.localScale = new Vector3(0.1f, 1f, 0.05f);
    windowPannelTwo.transform.SetParent(window.transform);
    PlaceObject(window, windowPannelTwo, SideOne.right, SideTwo.back, SideThree.nothing, 0f, -0.1f, 0f);

    GameObject windowPannelThree = GameObject.CreatePrimitive(PrimitiveType.Cube);
    windowPannelThree.name = windowName + "PannelThree";
    windowPannelThree.transform.localScale = new Vector3(1.25f, 0.1f, 0.05f);
    windowPannelThree.transform.SetParent(window.transform);
    PlaceObject(window, windowPannelThree, SideOne.above, SideTwo.back, SideThree.nothing, -0.1f, -0.1f, 0f);

    GameObject windowPannelFour = GameObject.CreatePrimitive(PrimitiveType.Cube);
    windowPannelFour.name = windowName + "PannelFour";
    windowPannelFour.transform.localScale = new Vector3(1.25f, 0.1f, 0.05f);
    windowPannelFour.transform.SetParent(window.transform);
    PlaceObject(window, windowPannelFour, SideOne.below, SideTwo.back, SideThree.nothing, -0.1f, -0.1f, 0f);


}


}
