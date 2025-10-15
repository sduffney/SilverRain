using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float mouseSensitivity = 2f;
    public float groundCheckDistance = 0.3f;

    [Header("Looking Settings")]
    public Transform cameraTransform;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;
    
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private bool isGrounded;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool enableMovement = true;
    
    public LayerMask groundLayerMask;

    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Look.performed += OnLook;
        controls.Player.Look.canceled += OnLook;
        controls.Player.Jump.performed += OnJump;
        controls.Player.Jump.canceled += OnJump;
        controls.Player.Interact.performed += ctx => { /* Handle interaction */ };
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleGroundCheck();
    }

    private void FixedUpdate()
    {
        if (!enableMovement) return;
        HandleMovement();
        HandleJump();
    }

    private void HandleLook()
    {
        if (lookInput.magnitude > 0.1f)
        {
            transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

            xRotation -= lookInput.y * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void HandleGroundCheck()
    {
        // Check if the player is grounded using a raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayerMask);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
        Debug.Log("Is Grounded: " + isGrounded);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        velocity = moveDirection.normalized * moveSpeed;
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
    }

    private void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpInput = false;
        }
    }

    private void HandleInteract()
    {

    }

    // Input System Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
            Debug.Log("Jump input received");
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        enableMovement = enabled;
        if (!enabled)
        {
            movementInput = Vector2.zero;
            lookInput = Vector2.zero;
            jumpInput = false;
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void Teleport(Vector3 position)
    {
        rb.position = position;
        rb.linearVelocity = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector3.zero;
    }

}
