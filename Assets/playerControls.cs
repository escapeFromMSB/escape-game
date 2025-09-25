using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float gravity = -10f;
    public float jumpHeight = 1.25f;

    [Header("Look")]
    public float mouseSensitivity = 120f;
    public Transform cameraPivot; 

    float yVelocity;
    float xLook;  
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (cameraPivot == null) cameraPivot = GetComponentInChildren<Camera>()?.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up, mouseX);
        xLook = Mathf.Clamp(xLook - mouseY, -85f, 85f);
        if (cameraPivot) cameraPivot.localEulerAngles = new Vector3(xLook, 0f, 0f);

        // Movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * h + transform.forward * v) * moveSpeed;

        // Gravity & jump
        if (cc.isGrounded && yVelocity < 0f) yVelocity = -2f; // keep grounded
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;

        cc.Move(move * Time.deltaTime);
    }
}