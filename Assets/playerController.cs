using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;       
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float airControl = 0.6f;    

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -9.81f;     
    [SerializeField] private float jumpHeight = 1.25f;   
    [SerializeField] private float groundedStick = -2f;  
    [SerializeField] private float coyoteTime = 0.08f;   

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 120f; 
    [SerializeField] private Transform cameraPivot;      

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
    private float pitch;          
    private float lastGroundedTime;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (!cameraPivot) cameraPivot = GetComponentInChildren<Camera>()?.transform;

        
        cc.slopeLimit = slopeLimit;
        cc.stepOffset = stepOffset;
        cc.skinWidth  = skinWidth;

        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start() {}

    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        transform.Rotate(Vector3.up, mouseX);

        
        pitch = Mathf.Clamp(pitch - mouseY, -85f, 85f);
        if (cameraPivot)
        {
            var e = cameraPivot.localEulerAngles;
            cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0f, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        
        Vector3 planar = (transform.right * input.x + transform.forward * input.z) * speed;

        bool grounded = cc.isGrounded;
        if (grounded) lastGroundedTime = Time.time;

        
        if (grounded && yVelocity < 0f) yVelocity = groundedStick;

        
        bool canJump = grounded || (Time.time - lastGroundedTime) <= coyoteTime;

        if (canJump && Input.GetButtonDown("Jump"))
        {
            
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        
        yVelocity += gravity * Time.deltaTime;

        
        Vector3 finalPlanar = grounded ? planar : Vector3.Lerp(Vector3.zero, planar, airControl);
        Vector3 velocity = new Vector3(finalPlanar.x, yVelocity, finalPlanar.z);

        
        cc.Move(velocity * Time.deltaTime);
    }
}
