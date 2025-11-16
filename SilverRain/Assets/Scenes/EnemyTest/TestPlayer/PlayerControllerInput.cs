using UnityEngine;
using UnityEngine.InputSystem;  

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerInput : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0f, 3f, -5f);
    public float lookSensitivity = 2f;
    public float cameraFollowSmooth = 10f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private CharacterController controller;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private bool isGrounded;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Setup input actions
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Subscribe to input callbacks if you want events
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;

        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Disable();
    }

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Vector3 angles = cameraTransform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    private void Update()
    {
        HandleMovement();
        HandleGravity();
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    // ===== INPUT CALLBACKS =====

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>(); // x = horizontal, y = vertical
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>(); // x = mouseX, y = mouseY
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // ===== MOVEMENT & CAMERA =====

    private void HandleMovement()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // Move relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        if (move.magnitude > 1f) move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Rotate player to movement direction
        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // Apply vertical velocity (gravity/jump in HandleGravity)
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleCamera()
    {
        if (cameraTransform == null) return;

        // Look input (mouse / right stick)
        yaw += lookInput.x * lookSensitivity;
        pitch -= lookInput.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = transform.position + rotation * cameraOffset;

        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            desiredPosition,
            cameraFollowSmooth * Time.deltaTime
        );

        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }
}
