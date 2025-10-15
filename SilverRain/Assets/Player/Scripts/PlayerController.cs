using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float mouseSensitivity = 2f;
    public float groundCheckDistance = 0.3f;

    [Header("Looking Settings")]
    public Transform cameraTransform;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;
    
    private CharacterController characterController;
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private bool isGrounded;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool enableMovement = true;
    
    public LayerMask groundLayerMask;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        isGrounded = characterController.isGrounded;
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        characterController.Move(moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }

        velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        characterController.Move(velocity * Time.fixedDeltaTime);
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
