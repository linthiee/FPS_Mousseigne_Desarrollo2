using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSensitivity = 0.15f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1.1f;

    private InputSystem_Actions actions;
    private Rigidbody rb;
    private float yaw;

    // ---------- Inicialización ----------
    
    private void Awake ()
    {
        actions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        yaw = transform.eulerAngles.y;
    }

    private void OnEnable ()
    {
        // Así se activa el Mapa:
        actions.Player.Enable();
        
        // Así te suscribis a eventos
        actions.Player.Jump.performed += DoJump;
    }

    private void OnDisable ()
    {
        // Acordate de desuscribirte acá si te suscribiste en OnEnable
        actions.Player.Jump.performed -= DoJump;
        actions.Player.Disable();
    }

    private void OnDestroy ()
    {
        // Acordate de cerrarlo cuando se destruye.
        actions.Dispose();
    }

    // ---------- Usos ----------
    
    private void LateUpdate ()
    {
        // Acumularlo en Update para evita perder input cuando FixedUpdate no corre en ese frame
        Vector2 look = actions.Player.Look.ReadValue<Vector2>(); // Se lee constantemente el input, no se espera a un action
        yaw += look.x * lookSensitivity;
    }

    private void FixedUpdate ()
    {
        // Rotación
        Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(targetRotation);

        // Movimiento
        Vector2 move = actions.Player.Move.ReadValue<Vector2>(); // Se lee constantemente el input, no se espera a un action
        Vector3 direction = targetRotation * new Vector3(move.x, 0f, move.y);
        Vector3 velocity = direction * moveSpeed;
        
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    private void DoJump (InputAction.CallbackContext ctx)
    {
        if (IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded ()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
