using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;       // m/s
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float airControl = 0.6f;    // 0..1

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -9.81f;     // m/s^2 (negative down)
    [SerializeField] private float jumpHeight = 1.25f;   // meters
    [SerializeField] private float groundedStick = -2f;  // small downward bias when grounded
    [SerializeField] private float coyoteTime = 0.08f;   // optional feel improvement

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 120f; // deg/s at 1.0 mouse input
    [SerializeField] private Transform cameraPivot;      // head/camera holder

    [Header("Controller Tuning")]
    [SerializeField] private float slopeLimit = 45f;
    [SerializeField] private float stepOffset = 0.3f;
    [SerializeField] private float skinWidth = 0.08f;

	public Transform CameraPivot
	{
    	get => cameraPivot;
    	set => cameraPivot = value;
	}

    private CharacterController cc;
    private float yVelocity;
    private float pitch;          // camera X rotation
    private float lastGroundedTime;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (!cameraPivot) cameraPivot = GetComponentInChildren<Camera>()?.transform;

        // Ensure controller has sensible values
        cc.slopeLimit = slopeLimit;
        cc.stepOffset = stepOffset;
        cc.skinWidth  = skinWidth;

        // Cursor lock for mouselook
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start() {}

    void Update()
    {
        // --- Look ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yaw on body
        transform.Rotate(Vector3.up, mouseX);

        // pitch on camera
        pitch = Mathf.Clamp(pitch - mouseY, -85f, 85f);
        if (cameraPivot)
        {
            var e = cameraPivot.localEulerAngles;
            cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        // --- Move input ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0f, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // apply sprint
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        // world/character space move (relative to facing)
        Vector3 planar = (transform.right * input.x + transform.forward * input.z) * speed;

        bool grounded = cc.isGrounded;
        if (grounded) lastGroundedTime = Time.time;

        // --- Jump / gravity ---
        if (grounded && yVelocity < 0f) yVelocity = groundedStick;

        // coyote time allows a small grace window after leaving ground
        bool canJump = grounded || (Time.time - lastGroundedTime) <= coyoteTime;

        if (canJump && Input.GetButtonDown("Jump"))
        {
            // v = sqrt(2 g h) with g negative -> -2f*gravity is positive
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // gravity integration
        yVelocity += gravity * Time.deltaTime;

        // reduce air control
        Vector3 finalPlanar = grounded ? planar : Vector3.Lerp(Vector3.zero, planar, airControl);
        Vector3 velocity = new Vector3(finalPlanar.x, yVelocity, finalPlanar.z);

        // --- Move controller ---
        cc.Move(velocity * Time.deltaTime);
    }
}
