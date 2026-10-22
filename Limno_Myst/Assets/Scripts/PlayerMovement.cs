using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    // Normal movement speed
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float mouseSensitivity = 3.0f;

    [Header("Stamina Settings")]
    // The max amount of stamina the player can have
    [SerializeField] private float maxStamina = 100f;

    // How quickly stamina decreases while sprinting
    [SerializeField] private float staminaDrain = 15f;

    // How quickly stamina regenerates when not sprinting
    [SerializeField] private float staminaRegen = 10f;

    // Reference to the stamina bar in the Canvas
    [SerializeField] private StaminaBar staminaBar;

    // The player's current stamina amount
    private float currentStamina;

    // Stamina needed before the player can sprint again after running out
    [SerializeField] private float resumeThreshold = 20f;

    // True after stamina hits zero, until it recovers past the threshold
    private bool isExhausted;

    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private float angle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Get the Rigidbody attached to the player
        rb = GetComponent<Rigidbody>();

        // Prevent physics from rotating the player automatically
        rb.freezeRotation = true;

        // Lock the mouse to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Start with full stamina
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    private void Update()
    {
        // Read mouse movement to rotate the player and camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player horizontally
        transform.Rotate(0f, mouseX, 0f);

        // Limit how far the camera can look up or down
        angle = Mathf.Clamp(angle - mouseY, -70f, 70f);

        // Rotate the camera if one has been assigned
        if (playerCamera != null)
        {
            playerCamera.localEulerAngles = new Vector3(angle, 0f, 0f);
        }

        // Read the movement keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction relative to the player
        moveDirection = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;
        
        // Check whether either Shift key is being held
        bool sprintKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Check whether the player is moving
        bool isMoving = horizontalInput != 0 || verticalInput != 0;
        
        // Sprint only when Shift is held, the player is moving, and not exhausted
        if (sprintKey && isMoving && !isExhausted)
        {
            // Increase the player's movement speed
            moveSpeed = 7f * 2;

            // Drain stamina over time while sprinting
            currentStamina -= staminaDrain * Time.deltaTime;

            // Out of stamina (Lock Sprint)
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isExhausted = true;
            }
        }
        else
        {
            // Use the normal movement speed
            moveSpeed = 7f;

            // Regenerate stamina when not sprinting
            currentStamina += staminaRegen * Time.deltaTime;

            // Allow sprinting again once enough stamina has recovered
            if (isExhausted && currentStamina >= resumeThreshold)
            {
                isExhausted = false;
            }
        }

        // Keep stamina between zedro and its maximum value
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Send the current stamina amount to the UI bar
        if (staminaBar != null)
        {
            staminaBar.SetStamina(currentStamina, maxStamina);
        }
    }

    // FixedUpdate is used for physics-related movement
    private void FixedUpdate()
    {
        // Calculate the player's movement velocity
        Vector3 velocity = moveDirection * moveSpeed;
        
        // Apply movement while keeping the existing vertical velocity
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
}
