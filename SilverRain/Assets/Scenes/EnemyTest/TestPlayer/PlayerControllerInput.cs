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

    [Header("Shooting Visuals")]
    public LineRenderer shootLine;
    public float shootLineDuration = 0.05f;

    [Header("Shooting")]
    public float shootRange = 50f;
    public int shootDamage = 1;
    public float shootCooldown = 0.25f;
    public LayerMask shootMask = ~0; // everything by default

    private CharacterController controller;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private bool isGrounded;

    private float yaw;
    private float pitch;

    private float lastShootTime = -999f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        inputActions = new PlayerInputActions();

        if (shootLine != null)
        {
            shootLine.enabled = false;
        }

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;

        inputActions.Player.Jump.performed += OnJump;

        //  Fire
        inputActions.Player.Fire.performed += OnFire;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;

        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Fire.performed -= OnFire;

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
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        TryShoot();
    }

    // ===== MOVEMENT & CAMERA =====

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        if (move.magnitude > 1f) move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleCamera()
    {
        if (cameraTransform == null) return;

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

    // ===== SHOOTING =====

    private void TryShoot()
    {
        if (Time.time < lastShootTime + shootCooldown)
            return;

        lastShootTime = Time.time;

        if (cameraTransform == null)
        {
            Debug.LogWarning("No cameraTransform assigned for shooting.");
            return;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        Vector3 endPoint = ray.origin + ray.direction * shootRange;

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange, shootMask, QueryTriggerInteraction.Ignore))
        {
            endPoint = hit.point;

            Debug.Log($"Hit: {hit.collider.name}");

            var enemy = hit.collider.GetComponentInParent<EnemyStateManager>();
            if (enemy != null)
            {
                enemy.TakeDamage(shootDamage);
            }
        }

        // Debug line in Scene view
        Debug.DrawRay(ray.origin, ray.direction * shootRange, Color.red, 0.25f);

        // Visible line in Game view via LineRenderer
        if (shootLine != null)
        {
            StartCoroutine(ShowShootLine(ray.origin, endPoint));
        }


    }
    private System.Collections.IEnumerator ShowShootLine(Vector3 start, Vector3 end)
    {
        shootLine.positionCount = 2;
        shootLine.SetPosition(0, start);
        shootLine.SetPosition(1, end);
        shootLine.enabled = true;

        yield return new WaitForSeconds(shootLineDuration);

        shootLine.enabled = false;
    }
}
