using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float mouseSensitivity = 2f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayerMask = 1;

    [Header("Looking Settings")]
    public Transform cameraTransform;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;
    
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool jumpInput;
    private float xRotation = 0f;
    private bool isGrounded = false;
    private bool enableMovement = true;

    public bool IsGrounded => isGrounded;
    public Vector3 Velocity => rb.linearVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        HandleCheckGround();
        HandleLook();
    }

    private void FixedUpdate()
    {
        if (!enableMovement) return;
        HandleMovement();
        HandleJump();
    }

    private void HandleCheckGround()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(rayStart, Vector3.down, groundCheckDistance + 0.1f, groundLayerMask);
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

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        Vector3 currentVelocity = rb.linearVelocity;

        if (jumpInput && isGrounded)
        {
            currentVelocity.y = jumpForce;
            jumpInput = false;
        }

        if (!isGrounded)
        {
            currentVelocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }
        else if (currentVelocity.y < 0)
        {
            currentVelocity.y = 0;
        }

        currentVelocity.x = horizontalVelocity.x;
        currentVelocity.z = horizontalVelocity.z;

        rb.linearVelocity = currentVelocity;
    }

    private void HandleJump()
    {
        if (jumpInput && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpInput = false;
        }
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
