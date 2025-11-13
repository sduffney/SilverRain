using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float jumpHeight = 3f;
    public float gravityScale = 1f;
    public float mouseSensitivity = 2f;

    [Header("Looking Settings")]
    public Transform cameraTransform;
    public float rotationSmoothSpeed = 10f;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;
    
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private bool isGrounded;
    private float xRotation = 0f;

    private PlayerInput playerInput;

    [Header("Weapons")]
    public SwordWeaponController swordPrefab;
    //public GunWeaponController gunController;      // Angelica added 
    //public GrenadeWeaponController grenadeController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Update()
    {
        //if (Time.timeScale == 0f) FreezePlayer(); else UnfreezePlayer();

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            print("Sword Activated!!!");
            var sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
            sword.player = transform;
            sword.Activate();
        }
            

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            //gunController.Activate();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            //grenadeController.Activate();
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.right * movementInput.x + transform.forward * movementInput.y;
        Vector3 targetVelocity = moveDirection.normalized * moveSpeed;
        Vector3 velocityChange = new(targetVelocity.x - rb.linearVelocity.x, 0, targetVelocity.z - rb.linearVelocity.z);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (lookInput.magnitude > 0.1f)
        {
            transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

            xRotation -= lookInput.y * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
            Quaternion targetRotation = Quaternion.Euler(xRotation, 0f, 0f);
            cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
        }
    }


    public void FreezePlayer()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //ResetVelocity();
        playerInput.enabled = false;
        //rb.isKinematic = true;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void UnfreezePlayer()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //ResetVelocity();
        playerInput.enabled = true;
        //rb.isKinematic = false;
        //rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
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
        if (context.performed && isGrounded)
        {
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public void EnemyKilled(string enemyType)
    {
        FindAnyObjectByType<HUDController>().SpownKillInfo(enemyType);
    }

    public void AddBuff(string buff)
    {
        FindAnyObjectByType<HUDController>().AddBuff(buff);
    }
}
