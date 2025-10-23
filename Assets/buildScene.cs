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
        GameObject cylinderOne = BuildCylinder(firstFloor, "cylinderOne", new Vector3(1.25f, 1.5f, 1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.75f, 8f, 0f);
        GameObject cylinderTwo = BuildCylinder(firstFloor, "cylinderTwo", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -0.68f, 8.9f, 0f);
        cylinderTwo.transform.Rotate(0f, -45f, 0f);
        GameObject cylinderThree = BuildCylinder(firstFloor, "cylinderThree", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -3f, 12.95f, 0f);
        cylinderThree.transform.Rotate(0f, -45f, 0f);
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
        GameObject wallEight = BuildWall(firstFloorPlaneTwo, "wallEight", new Vector3 (1.5f, 3f, 1f), SideOne.right,  SideTwo.front, SideThree.nothing, 0.85f, 8.8f, 0f);
        GameObject wallNine = BuildWall(firstFloor, "wallNine", new Vector3(1f, 3f, 13f), SideOne.left,  SideTwo.front, SideThree.nothing, -13.25f, 0f, 0f);
        GameObject wallTen = BuildWall(firstFloor, "wallTen", new Vector3(10f, 3f, 1f), SideOne.left,  SideTwo.front, SideThree.nothing, -3.25f, 13f, 0f);
        GameObject wallAboveCylinder = BuildWall(firstFloor, "wallAboveCylinder", new Vector3(0.5f, 0.5f, 13f), SideOne.left,  SideTwo.front, SideThree.above, -4f, 0f, 2.5f);
        GameObject wallEleven = BuildWall(firstFloor, "wallEleven", new Vector3(3f, 3f, 1f), SideOne.left,  SideTwo.front, SideThree.nothing, -0.9f, 14f, 0f);
        GameObject wallTwelve = BuildWall(firstFloor, "wallTwelve", new Vector3(0.5f, 3f, 6f), SideOne.left,  SideTwo.front, SideThree.nothing, -1f, 15.5f, 0f);
        GameObject wallThirteen = BuildWall(firstFloor, "wallThirteen", new Vector3(3f, 3f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, -1f, 22f, 0f);
        // Rotate 90° around Y relative to current rotation
        wallEight.transform.Rotate(0f, 45f, 0f);
        wallEleven.transform.Rotate(0f, 45f,0);

    
    

        // --- DOORS ---
        BuildDoor(firstFloor, "doorOne",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 0f, 0f,  hingeOnLeft: true,  openCW: false);
        BuildDoor(firstFloor, "doorTwo",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 0f, 0f, hingeOnLeft: false, openCW: true);
        BuildDoor(firstFloor, "doorThree",new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 2.9f, 0f,  hingeOnLeft: true,  openCW: false);
        BuildDoor(firstFloor, "doorFour", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 2.9f, 0f, hingeOnLeft: false, openCW: true);
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
        SpawnPlayer(firstFloor, new Vector3(-24f, -20f)); 
        
        //puzzle cube
        BuildPuzzleCube(firstFloor, "PuzzleCube_A", new Vector3(1.2f, 1.2f, 1.2f),
            SideOne.left, SideTwo.front, SideThree.nothing,
            -6.5f, 6.5f, 0f,
            "PuzzleSample");

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

void BuildDoor(GameObject floor, string doorName, Vector3 scale,
               SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
               float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset,
               bool hingeOnLeft, bool openCW)
{
    // --- create the door (visual cube) ---
    GameObject doorVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
    doorVisual.name = doorName;
    doorVisual.transform.localScale = scale;

    // place the visual door on the plane (centered)
    PlaceObjectOnPlane(floor, doorVisual, sideOne, sideTwo, sideThree,
                       edgeOneOffset, edgeTwoOffset, edgeThreeOffset);

    addGlassMaterial(doorVisual);

    // --- make a hinge pivot on the chosen edge ---
    var rend = doorVisual.GetComponent<Renderer>();
    float halfWidth = rend.bounds.extents.x; // assuming X is door width
    float hingeSign = hingeOnLeft ? -1f : +1f;

    Vector3 hingePos = new Vector3(
        doorVisual.transform.position.x + hingeSign * halfWidth,
        doorVisual.transform.position.y,
        doorVisual.transform.position.z
    );

    GameObject pivot = new GameObject(doorName + "_Pivot");
    pivot.transform.position = hingePos;
    pivot.transform.rotation = doorVisual.transform.rotation;

    // parent the visual under the pivot (keeps world pose)
    doorVisual.transform.SetParent(pivot.transform, worldPositionStays: true);

    // ensure the visual blocks when closed
    var blockingCol = doorVisual.GetComponent<Collider>();
    blockingCol.enabled = true;

    // add trigger & script to pivot
    var trigger = pivot.AddComponent<BoxCollider>(); // set to trigger in Initialize
    var auto    = pivot.AddComponent<DoorAutoOpen>();
    auto.Initialize(doorVisual.transform, blockingCol, openCW); // <-- opposite settings per panel

    // --- decorative panels (same as before) ---
    GameObject pannelOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelOne.name = doorName + "PannelOne";
    pannelOne.transform.localScale = new Vector3(0.1f, 2f, 0.05f);
    pannelOne.transform.SetParent(doorVisual.transform);
    PlaceObject(doorVisual, pannelOne, SideOne.left, SideTwo.back, SideThree.nothing, 0f, -0.1f, 0f);

    GameObject pannelTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelTwo.name = doorName + "PannelTwo";
    pannelTwo.transform.localScale = new Vector3(0.1f, 2f, 0.05f);
    pannelTwo.transform.SetParent(doorVisual.transform);
    PlaceObject(doorVisual, pannelTwo, SideOne.right, SideTwo.back, SideThree.nothing, 0f, -0.1f, 0f);

    GameObject pannelThree = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelThree.name = doorName + "PannelThree";
    pannelThree.transform.localScale = new Vector3(1f, 0.1f, 0.05f);
    pannelThree.transform.SetParent(doorVisual.transform);
    PlaceObject(doorVisual, pannelThree, SideOne.above, SideTwo.back, SideThree.nothing, -0.1f, -0.1f, 0f);

    GameObject pannelFour = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pannelFour.name = doorName + "PannelFour";
    pannelFour.transform.localScale = new Vector3(1f, 0.1f, 0.05f);
    pannelFour.transform.SetParent(doorVisual.transform);
    PlaceObject(doorVisual, pannelFour, SideOne.below, SideTwo.back, SideThree.nothing, -0.1f, -0.1f, 0f);

    // make panels non-blocking so only the main door collider matters
    DisableColliderIfAny(pannelOne);
    DisableColliderIfAny(pannelTwo);
    DisableColliderIfAny(pannelThree);
    DisableColliderIfAny(pannelFour);
}

void DisableColliderIfAny(GameObject go)
{
    var col = go.GetComponent<Collider>();
    if (col) col.enabled = false;
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

GameObject BuildPuzzleCube(GameObject floor, string name, Vector3 localScale,
    SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
    float off1, float off2, float off3,
    string puzzleSceneName)
{
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.name = name;
    cube.transform.localScale = localScale;

    PlaceObjectOnPlane(floor, cube, sideOne, sideTwo, sideThree, off1, off2, off3);

    addMaterial(cube, new Color(0.2f, 0.5f, 1f));

    var inter = cube.AddComponent<InteractablePuzzle>();
    inter.Configure(puzzleSceneName, cube.transform);
    
    return cube;
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


GameObject CreateDefaultPlayer(Vector3 spawnPos)
{
    // Root
    GameObject playerRoot = new GameObject("Player");
    playerRoot.transform.position = spawnPos;

    // CharacterController
    var cc = playerRoot.AddComponent<CharacterController>();
    cc.height = 2f;
    cc.radius = 0.4f;
    cc.center = new Vector3(0f, 1f, 0f);

    // Camera: reuse MainCamera if it exists; otherwise create one
    Camera existing = Camera.main;
    GameObject camGO;
    if (existing != null)
    {
        camGO = existing.gameObject;
        // Ensure it has the right tag and component
        camGO.tag = "MainCamera";
        // Parent it under the player and place at head height
        camGO.transform.SetParent(playerRoot.transform, worldPositionStays: false);
        camGO.transform.localPosition = new Vector3(0f, 1.6f, 0f);
        camGO.transform.localRotation = Quaternion.identity;
    }
    else
    {
        camGO = new GameObject("PlayerCamera");
        camGO.tag = "MainCamera";
        camGO.AddComponent<Camera>();
        camGO.transform.SetParent(playerRoot.transform, worldPositionStays: false);
        camGO.transform.localPosition = new Vector3(0f, 1.6f, 0f);
    }

    // Controller (you'll paste your PlayerController next)
    var controller = playerRoot.AddComponent<PlayerController>();
    controller.CameraPivot = camGO.transform;

    return playerRoot;
}


void SpawnPlayer(GameObject floor, Vector3 offsetXZ)
{
    // Put the player a little above the floor so it settles
    var rend = floor.GetComponent<Renderer>();
    float topY = rend ? rend.bounds.max.y : floor.transform.position.y;
    Vector3 spawn = new Vector3(offsetXZ.x, topY + 0.2f, offsetXZ.y);
    CreateDefaultPlayer(spawn);
}

}
