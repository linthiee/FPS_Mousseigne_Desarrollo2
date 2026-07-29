using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Rigidbody rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 1.4f;
    [SerializeField] private Transform playerCamera;

    private bool isGrounded;

    private InputSystem_Actions actions;

    [Header("Movement")] private Vector3 movementInput;
    private float velocity = 5.0f;

    [Header("Jump Stats")] private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    [Header("Camera")] public Vector2 cameraSensitivity = new Vector2(0.2f, 0.2f);
    private Vector2 look;
    private float yaw;
    private float pitch;

    private void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.Jump.performed += DoJump;
    }

    public void Awake()
    {
        actions = new InputSystem_Actions();
        yaw = transform.eulerAngles.y;
        pitch = 0f;
    }

    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LateUpdate()
    {
        look = actions.Player.Look.ReadValue<Vector2>();
        yaw += look.x * cameraSensitivity.x;
        pitch -= look.y * cameraSensitivity.y;

        pitch = Mathf.Clamp(pitch, -90f, 90f);
    }

    public void FixedUpdate()
    {
        Vector2 move = actions.Player.Move.ReadValue<Vector2>();
        movementInput = new Vector3(move.x, 0f, move.y).normalized;

        HandleCameraRotate();
        HandleMovement();
    }

    private void OnDisable()
    {
        actions.Player.Jump.performed -= DoJump;

        actions.Player.Disable();
    }

    private void OnDestroy()
    {
        actions.Dispose();
    }

    private void DoJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            verticalVelocity = jumpHeight;
        }
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        rb.AddForce(-(new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z) * 0.75f), ForceMode.VelocityChange);

        if (isGrounded && verticalVelocity >= 0.0f)
        {
            movementInput.y = verticalVelocity;
            verticalVelocity = 0.0f;
        }

        rb.AddRelativeForce(movementInput * velocity, ForceMode.VelocityChange);
    }

    private void HandleCameraRotate()
    {
        Quaternion bodyRotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(bodyRotation);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}