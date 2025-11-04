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

        Color customBlue = new Color(70f / 255f, 130f / 255f, 180f / 255f);
        Color customWhite = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        Color customMint = new Color(153f / 255f, 237f / 255f, 195f / 255f);


        //--- create the  first floor (plane) ---
        // createPrimitive creates a 3D object 
        GameObject firstFloor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        firstFloor.name = "firstFloor";
        GameObject secondFloor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        secondFloor.name = "secondFloor";
        ApplySpeckledTexture(firstFloor);
       

        // underside
        var underside = GameObject.CreatePrimitive(PrimitiveType.Plane);
        underside.name = "secondFloor_Underside";
        underside.transform.SetParent(secondFloor.transform, false);
        underside.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        underside.transform.localScale    = Vector3.one;

        // set the position
        // vector3 means 3 coordinates
        firstFloor.transform.position = new Vector3 (0,0,0);
        secondFloor.transform.position = new Vector3 (0, 3f ,0);

        // set scale to normal scale 
        // f means float. needs to be float and not double due to performance because float is faster
        // *** in unity, 1 unit = 1 meter. the scale (1,1,1) is 10x10 by default (for a plane it isx and z axis). ***
        firstFloor.transform.localScale = new Vector3 (5f,1f,5f); //plane is 50x50
        secondFloor.transform.localScale = new Vector3 (5f,1f,5f); //plane is 50x50

        //another first floor plane 
        GameObject firstFloorPlaneTwo = GameObject.CreatePrimitive(PrimitiveType.Plane);
        firstFloorPlaneTwo.name = "firstFloorPlaneTwo";
        firstFloorPlaneTwo.transform.position = new Vector3 (-50,0,0);
        firstFloorPlaneTwo.transform.localScale = new Vector3 (5f,1f,5f);
         ApplySpeckledTexture(firstFloorPlaneTwo);

         // elevator platform 
        Transform elevatorPlaform = BuildElevatorPlatform(firstFloor, "elevatorPlatform", new Vector3 (2f, 0.1f, 2f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 17f, 0f);

        //cylinder
        GameObject cylinderOne = BuildCylinder(firstFloor, "cylinderOne", new Vector3(1.25f, 1.5f, 1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.75f, 8f, 0f, customMint);
        GameObject cylinderTwo = BuildCylinder(firstFloor, "cylinderTwo", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -0.68f, 8.9f, 0f, customBlue);
        cylinderTwo.transform.Rotate(0f, -45f, 0f);
        GameObject cylinderThree = BuildCylinder(firstFloor, "cylinderThree", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -3f, 12.95f, 0f, customWhite);
        cylinderThree.transform.Rotate(0f, -45f, 0f);
        GameObject cylinderFour = BuildCylinder(firstFloor, "cylinderFour", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -0.9f, 15.15f, 0f, customWhite);
        //try rotating this

        // --- WALLS ---
        GameObject wallOne = BuildWall(firstFloor, "wallOne", new Vector3(1f, 3f, 9f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 0f, 0f, customBlue);
        GameObject wallTwo = BuildWall(firstFloor, "wallTwo", new Vector3 (0.5f, 3f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, -5f, 0f, 0f, customBlue);
        GameObject wallThree = BuildWall(firstFloor, "wallThree", new Vector3 (1f, 3f, 3f), SideOne.left,  SideTwo.front, SideThree.nothing, -4f, 0f, 0f, customBlue);
        GameObject wallOneBesideDoorTwo = BuildWall(firstFloor, "wallOneBesideDoorTwo", new Vector3 (0.5f, 3f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1f, 0f, 0f, customWhite);
        GameObject wallTwoBesideDoorTwo = BuildWall(firstFloor, "wallTwoBesideDoorTwo", new Vector3 (0.5f, 3f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -3.5f, 0f, 0f, customWhite);
        GameObject wallFour = BuildWall(firstFloor, "wallFour", new Vector3(1f, 3f, 6f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 0f, 0f, customWhite);
        GameObject wallFive = BuildWall(firstFloor, "wallFive", new Vector3(8f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, -5f, 0f, 2.5f, customWhite);
        GameObject wallSix = BuildWall(firstFloor, "wallSix", new Vector3(8f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, -5f, 0f, 0f, customWhite);
        GameObject wallSeven = BuildWall(firstFloor, "wallSeven", new Vector3 (1.5f, 3f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, -11.75f, 0f, 0f, customWhite);
        GameObject wallEight = BuildWall(firstFloorPlaneTwo, "wallEight", new Vector3 (1.5f, 3f, 1f), SideOne.right,  SideTwo.front, SideThree.nothing, 0.85f, 8.8f, 0f, customBlue);
        GameObject wallNine = BuildWall(firstFloor, "wallNine", new Vector3(1f, 3f, 13f), SideOne.left,  SideTwo.front, SideThree.nothing, -13.25f, 0f, 0f, customMint);
        GameObject wallTen = BuildWall(firstFloor, "wallTen", new Vector3(10f, 3f, 1f), SideOne.left,  SideTwo.front, SideThree.nothing, -3.25f, 13f, 0f, customWhite);
        GameObject wallAboveCylinder = BuildWall(firstFloor, "wallAboveCylinder", new Vector3(0.5f, 0.5f, 13f), SideOne.left,  SideTwo.front, SideThree.above, -4f, 0f, 2.5f, customWhite);
        GameObject wallEleven = BuildWall(firstFloor, "wallEleven", new Vector3(3f, 3f, 1f), SideOne.left,  SideTwo.front, SideThree.nothing, -0.9f, 14f, 0f, customWhite);
        GameObject wallTwelve = BuildWall(firstFloor, "wallTwelve", new Vector3(0.5f, 3f, 6f), SideOne.left,  SideTwo.front, SideThree.nothing, -1f, 15.5f, 0f, customMint);
        GameObject wallThirteen = BuildWall(firstFloor, "wallThirteen", new Vector3(1f, 3f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 21f, 0f, customWhite);
        //walls for back windows
        GameObject wallFifteen = BuildWall(firstFloor, "wallFifteen", new Vector3(2.75f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, 2.5f, 21f, 2.5f, customWhite);
        GameObject wallSixteen = BuildWall(firstFloor, "wallSixteen", new Vector3(2.75f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, 2.5f, 21f, 0f, customWhite);

        GameObject wallSeventeen = BuildWall(firstFloor, "wallSeventeen", new Vector3(1f, 7f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 21f, 0f, customBlue);
        //walls near elevator 
        GameObject wallEighteen = BuildWall(firstFloor, "wallEighteen", new Vector3(0.5f, 7f, 2f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 19f, 0f, customWhite);
        GameObject wallNineteen = BuildWall(firstFloor, "wallNineteen", new Vector3(0.5f, 7f, 2f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 15f, 0f, customWhite);
        GameObject wallTwenty = BuildWall(firstFloor, "wallTwenty", new Vector3(2f, 7f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 5.5f, 16.5f, 0f, customWhite);
        GameObject wallTwentyOne = BuildWall(firstFloor, "wallTwentyOne", new Vector3(2f, 7f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 5.5f, 19f, 0f, customWhite);
        GameObject wallTwentyTwo = BuildWall(firstFloor, "wallTwentyTwo", new Vector3(0.5f, 7f, 3f), SideOne.left,  SideTwo.front, SideThree.nothing, 5.5f, 17f, 0f, customWhite);
        // Rotate 90° around Y relative to current rotation
        wallEight.transform.Rotate(0f, 45f, 0f);
        wallEleven.transform.Rotate(0f, 45f, 0f);



        // --- DOORS ---
        BuildDoor(firstFloor, "doorOne",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 0f, 0f,  hingeOnLeft: true,  openCW: false);
        BuildDoor(firstFloor, "doorTwo",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 0f, 0f, hingeOnLeft: false, openCW: true);
        BuildDoor(firstFloor, "doorThree", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 2.9f, 0f,  hingeOnLeft: true,  openCW: false);
        BuildDoor(firstFloor, "doorFour", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 2.9f, 0f, hingeOnLeft: false, openCW: true);
        // --- glass pannels besides doors ---
        BuildGlassPannel(firstFloor, "glassPannelOne", new Vector3 (0.5f, 2f, 0.1f), SideOne.left, SideTwo.front, SideThree.nothing, -1f, 2.9f, 0f);
        BuildGlassPannel(firstFloor, "glassPannelTwo", new Vector3 (0.5f, 2f, 0.1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.5f, 2.9f, 0f); 
        // -- elevator doors -- 
        Transform leftDoor = BuildElevatorDoor(firstFloor, "elevatorDoorOne", new Vector3 (0.1f, 2f, 1), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 18f, 0f, null);
        Transform rightDoor = BuildElevatorDoor(firstFloor, "elevatorDoorOne", new Vector3 (0.1f, 2f, 1), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 17f, 0f, leftDoor);


        // --- front windows ---
        GameObject windowOne = BuildWindow(wallFive, "windowOne", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -0.5f, -0.05f, 0f);
        GameObject windowTwo = BuildWindow(wallSix, "windowTwo", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -0.5f, -0.05f, 0f);
        GameObject windowThree = BuildWindow(wallFive, "windowThree", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -1.75f, -0.05f, 0f);
        GameObject windowFour = BuildWindow(wallSix, "windowFour", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -1.75f, -0.05f, 0f);
        GameObject windowFive = BuildWindow(wallFive, "windowFive", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -3f, -0.05f, 0f);
        GameObject windowSix = BuildWindow(wallSix, "windowSix", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -3f, -0.05f, 0f);
        GameObject windowSeven = BuildWindow(wallFive, "windowSeven", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -4.25f, -0.05f, 0f);
        GameObject windowEight = BuildWindow(wallSix, "windowEight", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -4.25f, -0.05f, 0f);
        GameObject windowNine = BuildWindow(wallFive, "windowNine", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.below, -5.5f, -0.05f, 0f);
        GameObject windowTen = BuildWindow(wallSix, "windowTen", new Vector3 (1.25f, 1f, 0.1f), SideOne.left, SideTwo.front, SideThree.above, -5.5f, -0.05f, 0f);
        // --- back windows --
        GameObject windowEleven = BuildWindow(wallFifteen, "windowEleven", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.below, -0.25f, -0.05f, 0f);
        GameObject windowTwelve = BuildWindow(wallSixteen, "windowTwelve", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.above, -0.25f, -0.05f, 0f);
        GameObject windowThirteen = BuildWindow(wallFifteen, "windowThirteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.below, -1.5f, -0.05f, 0f);
        GameObject windowFourteen = BuildWindow(wallSixteen, "windowFourteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.above, -1.5f, -0.05f, 0f);

        windowEleven.transform.Rotate(0f, 180f, 0f);
        windowTwelve.transform.Rotate(0f, 180f, 0f);
        windowThirteen.transform.Rotate(0f, 180f, 0f);
        windowFourteen.transform.Rotate(0f, 180f, 0f);

     


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
                float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset, Color color){
    // calculate object positions from the plane’s current size and center, instead of hard-coding numbers.
    // this way we can update the plane size if needed without messing everyting else up
    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    wall.name = wallName;
    wall.transform.localScale = scale;
    PlaceObjectOnPlane(floor, wall, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);
    addMaterial(wall, color);
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
                        float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset, Color color){
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.name = cylinderName;
        cylinder.transform.localScale = scale;
        PlaceObjectOnPlane(floor, cylinder, sideOne, sideTwo, sideThree, edgeOneOffset, edgeTwoOffset, edgeThreeOffset);
        addMaterial(cylinder, color);
        return cylinder;
    }


GameObject BuildWindow(GameObject anchorObj, string windowName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree, 
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

    return window;
}

Transform BuildElevatorDoor(GameObject floor, string doorName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, SideThree sideThree, float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset, Transform counterpartDoor){
 // --- create the door (visual cube) ---
    GameObject doorVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
    doorVisual.name = doorName;
    doorVisual.transform.localScale = scale;

    // place the visual door on the plane (centered)
    PlaceObjectOnPlane(floor, doorVisual, sideOne, sideTwo, sideThree,
                       edgeOneOffset, edgeTwoOffset, edgeThreeOffset);

    //temp material. will be changed later. 
    addGlassMaterial(doorVisual);


    // --- create sensor (child) for trigger events ---
    GameObject sensor = new GameObject(doorName + "_Sensor");
    sensor.transform.SetParent(doorVisual.transform, worldPositionStays: true);
    sensor.transform.position = doorVisual.transform.position; // align with door

    // Trigger collider (make it a bit larger than the doorway)
    var trig = sensor.AddComponent<BoxCollider>();
    trig.isTrigger = true;
    trig.size = doorVisual.transform.localScale + new Vector3(1.0f, 1.0f, 1.0f); // expand as needed

    // Rigidbody required for trigger callbacks
    var rb = sensor.AddComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;

    // Door controller on sensor; tell it which transform to move
    // this is where the script is called. 
    // Door controller on sensor; tell it which transform to move
    var controller = sensor.AddComponent<ElevatorDoor>();
    controller.doorTarget = doorVisual.transform;
    controller.counterpartDoor = counterpartDoor; 

    return doorVisual.transform;
}

Transform BuildElevatorPlatform(GameObject floor, string platformName, Vector3 scale, SideOne sideOne, SideTwo sideTwo, 
SideThree sideThree, float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset){
    GameObject elevatorPlatform =  GameObject.CreatePrimitive(PrimitiveType.Cube);
    elevatorPlatform.name = platformName;
    elevatorPlatform.transform.localScale = scale;
    PlaceObjectOnPlane(floor, elevatorPlatform, sideOne, sideTwo, sideThree,
                       edgeOneOffset, edgeTwoOffset, edgeThreeOffset);

    //temp material. will be changed later. 
    addGlassMaterial(elevatorPlatform);

    // --- create sensor (child) for trigger events ---
    GameObject sensor = new GameObject(platformName + "_Sensor");
    sensor.transform.SetParent(elevatorPlatform.transform, worldPositionStays: true);
    sensor.transform.position = elevatorPlatform.transform.position; 

    // Trigger collider 
    var trig = sensor.AddComponent<BoxCollider>();
    trig.isTrigger = true;
    trig.size = elevatorPlatform.transform.localScale;

    // Rigidbody required for trigger callbacks
    var rb = sensor.AddComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.useGravity = false;

    // Door controller on sensor; tell it which transform to move
    // this is where the script is called. 
    // Door controller on sensor; tell it which transform to move
    var controller = sensor.AddComponent<ElevatorController>();
    controller.platformTarget = elevatorPlatform.transform;

    return elevatorPlatform.transform;

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



    void ApplySpeckledTexture(GameObject floor)
    {
        Texture2D tex = MakeSpeckledTexture(512, new Color(0.97f, 0.94f, 0.86f), Color.black, Color.white, 10000);
        tex.wrapMode = TextureWrapMode.Repeat;
        tex.filterMode = FilterMode.Trilinear;

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetTexture("_BaseMap", tex);
        mat.SetFloat("_Metallic", 0.05f);
        mat.SetFloat("_Smoothness", 0.3f);
        mat.SetColor("_BaseColor", new Color(0.97f, 0.94f, 0.86f));
        mat.SetTextureScale("_BaseMap", new Vector2(8, 8));

        floor.GetComponent<Renderer>().material = mat;
    }

    Texture2D MakeSpeckledTexture(int size, Color baseColor, Color speckleA, Color speckleB, int count)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[size * size];

        // Base color
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = baseColor;

        // Random speckles
        System.Random rng = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int x = rng.Next(size);
            int y = rng.Next(size);
            Color c = Color.Lerp(speckleA, speckleB, (float)rng.NextDouble());
            int radius = rng.Next(1, 3);

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    int px = (x + dx + size) % size;
                    int py = (y + dy + size) % size;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    if (dist <= radius)
                        pixels[py * size + px] = Color.Lerp(pixels[py * size + px], c, 0.7f);
                }
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

}
