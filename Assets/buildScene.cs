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

        Color customBlue  = new Color(70f / 255f, 130f / 255f, 180f / 255f);
        Color customWhite = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        Color customMint  = new Color(153f / 255f, 237f / 255f, 195f / 255f);

        //--- create the  first floor (plane) ---
        GameObject firstFloor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        firstFloor.name = "firstFloor";
        GameObject secondFloor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        secondFloor.name = "secondFloor";
        ApplySpeckledTexture(firstFloor);
        ApplySpeckledTexture(secondFloor);

        // underside
        var underside = GameObject.CreatePrimitive(PrimitiveType.Plane);
        underside.name = "secondFloor_Underside";
        underside.transform.SetParent(secondFloor.transform, false);
        underside.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        underside.transform.localScale    = Vector3.one;

        // positions
        firstFloor.transform.position = new Vector3 (0,0,0);
        secondFloor.transform.position = new Vector3 (-3.25f, 3.05f ,-3.25f);

        // scales
        firstFloor.transform.localScale = new Vector3 (5f,1f,5f);
        secondFloor.transform.localScale = new Vector3 (5f,1f,5f);

        // another first floor plane 
        GameObject firstFloorPlaneTwo = GameObject.CreatePrimitive(PrimitiveType.Plane);
        firstFloorPlaneTwo.name = "firstFloorPlaneTwo";
        firstFloorPlaneTwo.transform.position = new Vector3 (-50,0,0);
        firstFloorPlaneTwo.transform.localScale = new Vector3 (5f,1f,5f);
        ApplySpeckledTexture(firstFloorPlaneTwo);

        // second floor ceiling
        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Plane);
        roof.name = "firstFloorPlaneTwo";
        roof.transform.position = new Vector3 (-3.25f, 6.05f, 6.75f);
        roof.transform.localScale = new Vector3 (5f,1f,5f);

        // underside
        var underside2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        underside2.name = "secondFloor_Underside";
        underside2.transform.SetParent(roof.transform, false);
        underside2.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
        underside2.transform.localScale    = Vector3.one;

        // elevator platform  (TWEAKED OFFSETS)
        Transform elevatorPlaform = BuildElevatorPlatform(
            firstFloor, "elevatorPlatform", new Vector3(2f, 0.1f, 2f),
            SideOne.left, SideTwo.front, SideThree.above,
            5.25f, 17f, 2.93f);
        // --- COLLECT-ALL PUZZLE MANAGER ---
        var puzzleManagerGO = new GameObject("CollectAllPuzzleManager");
        var puzzle = puzzleManagerGO.AddComponent<CollectAllPuzzle>();
        puzzle.puzzleTitle = "Collect All Items";
        puzzle.Initialize(new string[] {
            "Keycard_Red",
            "Keycard_Blue",
            "Keycard_Green",
            "Battery_AA",
            "Crowbar",
            "Fuse"
        });

        // --- START CONSOLE (first floor, near elevator lobby but clear of walls) ---
        GameObject console = GameObject.CreatePrimitive(PrimitiveType.Cube);
        console.name = "PuzzleStartConsole";
        console.transform.localScale = new Vector3(0.6f, 1.0f, 0.4f);

        // Reuse your plane alignment helper to place it nicely
        PlaceObjectOnPlane(firstFloor, console,
            SideOne.left,  // X against left side of the floor area
            SideTwo.front, // Z near the front edge
            SideThree.above,
            6.5f, 16.0f, 0.0f);

        addMaterial(console, new Color(0.25f, 0.25f, 0.3f));

        // Add trigger + script
        var col = console.GetComponent<Collider>();
        col.isTrigger = true;
        var rb = console.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        var consoleScript = console.AddComponent<PuzzleConsole>();
        consoleScript.puzzle = puzzle;
        consoleScript.prompt = "Press E to begin the collection puzzle";

        // cylinders
        GameObject cylinderOne   = BuildCylinder(firstFloor, "cylinderOne",   new Vector3(1.25f, 1.5f, 1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.75f, 8f, 0f, customMint);
        GameObject cylinderTwo   = BuildCylinder(firstFloor, "cylinderTwo",   new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -0.68f, 8.9f, 0f, customBlue);
        cylinderTwo.transform.Rotate(0f, -45f, 0f);
        GameObject cylinderThree = BuildCylinder(firstFloor, "cylinderThree", new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -3f, 12.95f, 0f, customWhite);
        cylinderThree.transform.Rotate(0f, -45f, 0f);
        GameObject cylinderFour  = BuildCylinder(firstFloor, "cylinderFour",  new Vector3(0.25f, 1.5f, 0.5f), SideOne.left, SideTwo.front, SideThree.nothing, -0.9f, 15.15f, 0f, customWhite);

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
        // back windows
        GameObject wallFifteen = BuildWall(firstFloor, "wallFifteen", new Vector3(2.75f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, 2.5f, 21f, 2.5f, customWhite);
        GameObject wallSixteen = BuildWall(firstFloor, "wallSixteen", new Vector3(2.75f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, 2.5f, 21f, 0f, customWhite);

        GameObject wallSeventeen = BuildWall(firstFloor, "wallSeventeen", new Vector3(1f, 7f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 21f, 0f, customBlue);
        // walls near elevator 
        GameObject wallEighteen = BuildWall(firstFloor, "wallEighteen", new Vector3(0.5f, 7f, 2f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 19f, 0f, customWhite);
        GameObject wallNineteen = BuildWall(firstFloor, "wallNineteen", new Vector3(0.5f, 7f, 2f), SideOne.left,  SideTwo.front, SideThree.nothing, 3.5f, 15f, 0f, customWhite);
        GameObject wallTwenty = BuildWall(firstFloor, "wallTwenty", new Vector3(2f, 7f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 5.5f, 16.5f, 0f, customWhite);
        GameObject wallTwentyOne = BuildWall(firstFloor, "wallTwentyOne", new Vector3(2f, 7f, 0.5f), SideOne.left,  SideTwo.front, SideThree.nothing, 5.5f, 19f, 0f, customWhite);
        GameObject wallTwentyTwo = BuildWall(firstFloor, "wallTwentyTwo", new Vector3(0.5f, 7f, 3f), SideOne.left,  SideTwo.front, SideThree.nothing, 5.75f, 17f, 0f, customWhite);

        // Rotate
        wallEight.transform.Rotate(0f, 45f, 0f);
        wallEleven.transform.Rotate(0f, 45f, 0f);

        //--- SECOND FLOOR WALLS ---
        GameObject wallTwentyFour = BuildWall(secondFloor, "wallTwentyFour", new Vector3(0.25f, 3f, 8.25f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 10.0f, 0f, customMint);
        // back windows second floor
        GameObject wallTwentyFive = BuildWall(secondFloor, "wallTwentyFive", new Vector3(2.75f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.above, -0.75f, 24.25f, 2.5f, customWhite);
        GameObject wallTwentySix = BuildWall(secondFloor, "wallTwentySix", new Vector3(2.75f, 0.5f, 0.15f), SideOne.left,  SideTwo.front, SideThree.nothing, -0.75f, 24.25f, 0f, customWhite);

        GameObject wallTwentySeven = BuildWall(secondFloor, "wallTwentySeven", new Vector3(1f, 3f, 0.25f), SideOne.left,  SideTwo.front, SideThree.nothing, -3.25f, 24.25f, 0f, customWhite);
        GameObject wallTwentyEight = BuildWall(secondFloor, "wallTwentyEight", new Vector3(0.25f, 3f, 6f), SideOne.left,  SideTwo.front, SideThree.nothing, -4.25f, 18.25f, 0f, customMint);
        GameObject wallTwentyNine = BuildWall(secondFloor, "wallTwentyNine", new Vector3(0.5f, 3f, 0.25f), SideOne.left,  SideTwo.front, SideThree.nothing, -4.5f, 18.25f, 0f, customMint);
        GameObject wallThirty = BuildWall(secondFloor, "wallThirty", new Vector3(0.25f, 3f, 8.25f), SideOne.left,  SideTwo.front, SideThree.nothing, -5f, 10f, 0f, customWhite);
        // back walls
        GameObject wallThirtyOne = BuildWall(secondFloor, "wallThirtyOne", new Vector3(10f, 6f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.25f, 10f, 0f, customMint);
        GameObject wallThirtyTwo = BuildWall(secondFloor, "wallThirtyTwo", new Vector3(2f, 1f, 0.1f), SideOne.left,  SideTwo.front, SideThree.above, 0.25f, 10f, 2.5f, customMint);
        // MSB 228
        GameObject wallTwentyThree = BuildWall(secondFloor, "wallTwentyThree", new Vector3(0.25f, 6f, 10f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 0f, 0f, customWhite);
        GameObject wallThirtyFour = BuildWall(secondFloor, "wallThirtyFour", new Vector3(15f, 6f, 0.25f), SideOne.left,  SideTwo.front, SideThree.nothing, 0f, 0f, 0f, customWhite);
        GameObject wallThirtyFive = BuildWall(secondFloor, "wallThirtyFive", new Vector3(15f, 6f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.25f, 9.9f, 0f, customWhite);
        GameObject wallThirtySix = BuildWall(secondFloor, "wallThirtySix", new Vector3(0.1f, 6f, 15f), SideOne.left,  SideTwo.front, SideThree.nothing, -15f, 0f, 0f, customWhite);

        // --- DOORS ---
        BuildDoor(firstFloor, "doorOne",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 0f, 0f,  hingeOnLeft: true,  openCW: false);
        BuildDoor(firstFloor, "doorTwo",  new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 0f, 0f, hingeOnLeft: false, openCW: true);
        BuildDoor(firstFloor, "doorThree", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -1.5f, 2.9f, 0f,  hingeOnLeft: true,  openCW: false);
        BuildDoor(firstFloor, "doorFour", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -2.5f, 2.9f, 0f, hingeOnLeft: false, openCW: true);
        // door to MSB 228
        BuildDoor(secondFloor, "doorFive", new Vector3(1f, 2f, 0.1f), SideOne.left,  SideTwo.front, SideThree.nothing, -0.25f, 10f, 0f, hingeOnLeft: false, openCW: true);
        // glass panels
        BuildGlassPannel(firstFloor, "glassPannelOne", new Vector3 (0.5f, 2f, 0.1f), SideOne.left, SideTwo.front, SideThree.nothing, -1f, 2.9f, 0f);
        BuildGlassPannel(firstFloor, "glassPannelTwo", new Vector3 (0.5f, 2f, 0.1f), SideOne.left, SideTwo.front, SideThree.nothing, -3.5f, 2.9f, 0f); 
        // elevator doors 
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
        // back windows first floor
        GameObject windowEleven = BuildWindow(wallFifteen, "windowEleven", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.below, -0.25f, -0.05f, 0f);
        GameObject windowTwelve = BuildWindow(wallSixteen, "windowTwelve", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.above, -0.25f, -0.05f, 0f);
        GameObject windowThirteen = BuildWindow(wallFifteen, "windowThirteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.below, -1.5f, -0.05f, 0f);
        GameObject windowFourteen = BuildWindow(wallSixteen, "windowFourteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.above, -1.5f, -0.05f, 0f);

        // back windows second floor
        GameObject windowFifteen = BuildWindow(wallTwentyFive, "windowFifteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.below, -0.25f, -0.05f, 0f);
        GameObject windowSixteen = BuildWindow(wallTwentySix, "windowSixteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.above, -0.25f, -0.05f, 0f);
        GameObject windowSeventeen = BuildWindow(wallTwentyFive, "windowSeventeen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.below, -1.5f, -0.05f, 0f);
        GameObject windowEighteen = BuildWindow(wallTwentySix, "windowEighteen", new Vector3 (1.25f, 1f, 0.1f), SideOne.right, SideTwo.back, SideThree.above, -1.5f, -0.05f, 0f);

        windowEleven.transform.Rotate(0f, 180f, 0f);
        windowTwelve.transform.Rotate(0f, 180f, 0f);
        windowThirteen.transform.Rotate(0f, 180f, 0f);
        windowFourteen.transform.Rotate(0f, 180f, 0f);

        windowFifteen.transform.Rotate(0f, 180f, 0f);
        windowSixteen.transform.Rotate(0f, 180f, 0f);
        windowSeventeen.transform.Rotate(0f, 180f, 0f);
        windowEighteen.transform.Rotate(0f, 180f, 0f);

        // --- PLAYER ---
        SpawnPlayer(firstFloor, new Vector3(-24f, -20f)); 

        /*
        // --- PUZZLE CUBE ---
        BuildPuzzleCube(firstFloor, "PuzzleCube_A", new Vector3(1.2f, 1.2f, 1.2f),
            SideOne.left, SideTwo.front, SideThree.nothing,
            -6.5f, 6.5f, 0f,
            "PuzzleSample");
        */
        // --- SCATTERED PICKUPS (not grouped near the puzzle) ---
        // First floor, different rooms/corridors
        BuildPickupItem(firstFloor, "Keycard_Red",  new Vector3(0.35f, 0.12f, 0.35f),
            SideOne.left, SideTwo.front, SideThree.nothing, -9.0f,  3.0f, 0f,
            "keycard_red",  "Red Keycard", 1, new Color(0.85f, 0.2f, 0.2f));

        BuildPickupItem(firstFloor, "Keycard_Blue", new Vector3(0.35f, 0.12f, 0.35f),
            SideOne.left, SideTwo.front, SideThree.nothing, -2.0f, 11.5f, 0f,
            "keycard_blue", "Blue Keycard", 1, new Color(0.2f, 0.35f, 0.9f));

        BuildPickupItem(firstFloor, "Battery_AA",   new Vector3(0.25f, 0.15f, 0.25f),
            SideOne.left, SideTwo.front, SideThree.nothing, -12.0f, 18.0f, 0f,
            "battery", "Battery", 2, new Color(0.9f, 0.85f, 0.25f));

        // Near elevator lobby but not right at the player spawn
        BuildPickupItem(firstFloor, "Crowbar",      new Vector3(0.8f, 0.1f, 0.15f),
            SideOne.left, SideTwo.front, SideThree.nothing,  6.0f, 16.2f, 0f,
            "crowbar", "Crowbar", 1, new Color(0.6f, 0.3f, 0.1f));

        // Second floor hallway
        BuildPickupItem(secondFloor, "Fuse",        new Vector3(0.2f, 0.2f, 0.2f),
            SideOne.left, SideTwo.front, SideThree.nothing,  -2.0f,  9.6f, 0f,
            "fuse", "Fuse", 1, new Color(0.75f, 0.75f, 0.8f));

        BuildPickupItem(secondFloor, "Keycard_Green", new Vector3(0.35f, 0.12f, 0.35f),
            SideOne.left, SideTwo.front, SideThree.nothing,  -4.0f, 24.0f, 0f,
            "keycard_green", "Green Keycard", 1, new Color(0.2f, 0.8f, 0.3f));
    }

    void Update()
    {
    }

    // --- ALIGNMENT FUNCTIONS ---

    void PlaceObject(GameObject a, GameObject b, SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                     float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset)
    {
        Renderer anchorRenderer = a.GetComponent<Renderer>();
        Renderer objectRenderer = b.GetComponent<Renderer>();

        Vector3 halfA = anchorRenderer.bounds.extents;
        Vector3 halfB = objectRenderer.bounds.extents;

        Vector3 posA = a.transform.position;
        Vector3 pos = posA;

        float topA = anchorRenderer.bounds.max.y;
        float bottomA = anchorRenderer.bounds.min.y;

        switch (sideOne)
        {
            case SideOne.right: pos.x = posA.x + (halfA.x - halfB.x) + edgeOneOffset; break;
            case SideOne.left:  pos.x = posA.x - (halfA.x - halfB.x) - edgeOneOffset; break;
            case SideOne.front: pos.z = posA.z + (halfA.z - halfB.z) + edgeOneOffset; break;
            case SideOne.back:  pos.z = posA.z - (halfA.z - halfB.z) - edgeOneOffset; break;
            case SideOne.above: pos.y = topA + halfB.y + edgeOneOffset; break;
            case SideOne.below: pos.y = bottomA - halfB.y - edgeOneOffset; break;
        }

        switch (sideTwo)
        {
            case SideTwo.right: pos.x = posA.x + (halfA.x - halfB.x) + edgeTwoOffset; break;
            case SideTwo.left:  pos.x = posA.x - (halfA.x - halfB.x) - edgeTwoOffset; break;
            case SideTwo.front: pos.z = posA.z + (halfA.z - halfB.z) + edgeTwoOffset; break;
            case SideTwo.back:  pos.z = posA.z - (halfA.z - halfB.z) - edgeTwoOffset; break;
            case SideTwo.above: pos.y = topA + halfB.y + edgeTwoOffset; break;
            case SideTwo.below: pos.y = bottomA - halfB.y - edgeTwoOffset; break;
        }

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

        b.transform.position = pos;
    }

    void PlaceObjectOnPlane(GameObject a, GameObject b, SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                            float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset)
    {
        Renderer anchorRenderer = a.GetComponent<Renderer>();
        Renderer objectRenderer = b.GetComponent<Renderer>();

        Vector3 halfA = anchorRenderer.bounds.extents;
        Vector3 halfB = objectRenderer.bounds.extents;

        Vector3 posA = a.transform.position;
        Vector3 pos = posA;

        float topA = anchorRenderer.bounds.max.y;
        float bottomA = anchorRenderer.bounds.min.y;

        switch (sideOne)
        {
            case SideOne.right:
                pos.x = posA.x + (halfA.x - halfB.x) + edgeOneOffset;
                pos.y = topA + halfB.y;
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

        switch (sideTwo)
        {
            case SideTwo.right:
                pos.x = posA.x + (halfA.x - halfB.x) + edgeTwoOffset;
                pos.y = topA + halfB.y;
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

        switch (sideThree)
        {
            case SideThree.right:   pos.x = posA.x + (halfA.x - halfB.x) + edgeThreeOffset; break;
            case SideThree.left:    pos.x = posA.x - (halfA.x - halfB.x) - edgeThreeOffset; break;
            case SideThree.front:   pos.z = posA.z - (halfA.z - halfB.z) + edgeThreeOffset; break;
            case SideThree.back:    pos.z = posA.z + (halfA.z - halfB.z) - edgeThreeOffset; break;
            case SideThree.above:   pos.y = topA + halfB.y + edgeThreeOffset; break;
            case SideThree.below:   pos.y = bottomA - halfB.y - edgeThreeOffset; break;
            case SideThree.nothing: break;
        }

        b.transform.position = pos;
    }

    // --- MATERIAL HELPERS ---
    void addMaterial(GameObject obj, Color objColor){
        Renderer rend = obj.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = objColor;
        rend.material = mat;
    }

    void addGlassMaterial(GameObject obj){
        Renderer rend = obj.GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor", new Color(0.9f, 0.9f, 1f, 0.1f)); 
        mat.SetFloat("_Surface", 1f);  
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        mat.SetFloat("_Smoothness", 1f);
        mat.SetFloat("_Metallic", 1f);
        rend.material = mat;
    }

    // --- BUILDERS ---
    void BuildDoor(GameObject floor, string doorName, Vector3 scale,
                   SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
                   float edgeOneOffset, float edgeTwoOffset, float edgeThreeOffset,
                   bool hingeOnLeft, bool openCW)
    {
        GameObject doorVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doorVisual.name = doorName;
        doorVisual.transform.localScale = scale;

        PlaceObjectOnPlane(floor, doorVisual, sideOne, sideTwo, sideThree,
                           edgeOneOffset, edgeTwoOffset, edgeThreeOffset);

        addGlassMaterial(doorVisual);

        var rend = doorVisual.GetComponent<Renderer>();
        float halfWidth = rend.bounds.extents.x;
        float hingeSign = hingeOnLeft ? -1f : +1f;

        Vector3 hingePos = new Vector3(
            doorVisual.transform.position.x + hingeSign * halfWidth,
            doorVisual.transform.position.y,
            doorVisual.transform.position.z
        );

        GameObject pivot = new GameObject(doorName + "_Pivot");
        pivot.transform.position = hingePos;
        pivot.transform.rotation = doorVisual.transform.rotation;

        doorVisual.transform.SetParent(pivot.transform, worldPositionStays: true);

        var blockingCol = doorVisual.GetComponent<Collider>();
        blockingCol.enabled = true;

        var trigger = pivot.AddComponent<BoxCollider>();
        var auto    = pivot.AddComponent<DoorAutoOpen>();
        auto.Initialize(doorVisual.transform, blockingCol, openCW);

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

        GameObject windowPannelOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
        windowPannelOne.name = windowName + "PannelOne";
        windowPannelOne.transform.localScale = new Vector3(0.1f, 1f, 0.05f);
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
        GameObject doorVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
        doorVisual.name = doorName;
        doorVisual.transform.localScale = scale;

        PlaceObjectOnPlane(floor, doorVisual, sideOne, sideTwo, sideThree,
                           edgeOneOffset, edgeTwoOffset, edgeThreeOffset);

        addGlassMaterial(doorVisual);

        GameObject sensor = new GameObject(doorName + "_Sensor");
        sensor.transform.SetParent(doorVisual.transform, worldPositionStays: true);
        sensor.transform.position = doorVisual.transform.position;

        var trig = sensor.AddComponent<BoxCollider>();
        trig.isTrigger = true;
        trig.size = doorVisual.transform.localScale + new Vector3(1.0f, 1.0f, 1.0f);

        var rb = sensor.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

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

        addGlassMaterial(elevatorPlatform);

        GameObject sensor = new GameObject(platformName + "_Sensor");
        sensor.transform.SetParent(elevatorPlatform.transform);
        sensor.transform.position = elevatorPlatform.transform.position + new Vector3(1.0f, 1.0f, 1.0f); 

        var trig = sensor.AddComponent<BoxCollider>();
        trig.isTrigger = true;
        trig.size = elevatorPlatform.transform.localScale;

        var rb = sensor.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        var controller = sensor.AddComponent<ElevatorController>();
        controller.platformTarget = elevatorPlatform.transform;

        return elevatorPlatform.transform;
    }

    // --- PICKUP BUILDER ---
    GameObject BuildPickupItem(GameObject floor, string name, Vector3 localScale,
        SideOne sideOne, SideTwo sideTwo, SideThree sideThree,
        float off1, float off2, float off3,
        string itemId, string displayName, int amount, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.localScale = localScale;
        PlaceObjectOnPlane(floor, go, sideOne, sideTwo, sideThree, off1, off2, off3);
        addMaterial(go, color);

        // Add pickup component
        var pickup = go.AddComponent<InventoryPickup>();
        pickup.itemId = itemId;
        pickup.displayName = displayName;
        pickup.amount = amount;
        pickup.enablePulse = true; // pulses only when eligible (PlayerInteractor controls)

        // Collider is already present via CreatePrimitive
        // Keep it non-trigger; PlayerInteractor uses raycast, not trigger

        return go;
    }

    // --- PLAYER SPAWN / SETUP ---
    GameObject CreateDefaultPlayer(Vector3 spawnPos)
    {
        GameObject playerRoot = new GameObject("Player");
        playerRoot.transform.position = spawnPos;

        var cc = playerRoot.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.radius = 0.4f;
        cc.center = new Vector3(0f, 1f, 0f);

        Camera existing = Camera.main;
        GameObject camGO;
        if (existing != null)
        {
            camGO = existing.gameObject;
            camGO.tag = "MainCamera";
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
        camGO.AddComponent<CrosshairUI>();
        var controller = playerRoot.AddComponent<PlayerController>();
        controller.CameraPivot = camGO.transform;

        // Ensure interaction works out of the box
        var interactor = playerRoot.AddComponent<PlayerInteractor>();
        interactor.viewCamera = camGO.GetComponent<Camera>(); // if null, it will use Camera.main anyway

        // InventoryUI + PlayerInventory will be auto-created by PlayerInteractor/InventoryUI if missing

        return playerRoot;
    }

    void SpawnPlayer(GameObject floor, Vector3 offsetXZ)
    {
        var rend = floor.GetComponent<Renderer>();
        float topY = rend ? rend.bounds.max.y : floor.transform.position.y;
        Vector3 spawn = new Vector3(offsetXZ.x, topY + 3.1f, offsetXZ.y);
        CreateDefaultPlayer(spawn);
    }

    // --- SPECKLED FLOOR TEXTURE ---
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

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = baseColor;

        System.Random rng = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int x = rng.Next(size);
            int y = rng.Next(size);
            Color c = Color.Lerp(speckleA, speckleB, (float)rng.NextDouble());
            int radius = rng.Next(1, 3);

            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                int px = (x + dx + size) % size;
                int py = (y + dy + size) % size;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                if (dist <= radius)
                    pixels[py * size + px] = Color.Lerp(pixels[py * size + px], c, 0.7f);
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }
}
