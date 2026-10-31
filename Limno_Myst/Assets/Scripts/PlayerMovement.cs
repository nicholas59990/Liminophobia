using System.Collections;
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

    //Basic movement properties
    private Vector3 moveDirection;
    private Rigidbody rb;
    private float angle;
    private Collider playerCollider;

    //Manipulates the player's movement for hiding
    public bool IsHidden{get; private set;}
    public bool IsTransitioning {get; private set;}
    public HideInteractable CurrentSpot {get; private set;}

    [Header("Footsteps and Camera Bob")]
    
    [SerializeField] private float walkStepTime = 0.5f;
    [SerializeField] private float sprintStepTime = 0.3f;

    // How far the camera dips on each step
    [SerializeField] private float walkBobAmount = 0.04f;
    [SerializeField] private float sprintBobAmount = 0.08f;

    // Footsteps sound a bit higher when sprinting
    [SerializeField] private float sprintPitch = 1.15f;

    private AudioSource footstepSource;
    private float stepTimer;
    private Vector3 cameraStartPosition;
    private bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Get the Rigidbody attached to the player
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        // Prevent physics from rotating the player automatically
        rb.freezeRotation = true;

        // Lock the mouse to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = maxStamina;

        // Start with full stamina
        currentStamina = maxStamina;

        // Remember where the camera normally sits
        if (playerCamera != null)
        {
            cameraStartPosition = playerCamera.localPosition;
        }

        // Find the Audio Source on this object
        footstepSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {

        // Do not move or look around while dialogue is open
        if (DialogueManager.IsPlaying)
        {
            moveDirection = Vector3.zero;
            isSprinting = false;

            // Stop the footstep sound
            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }

            playerCamera.localPosition = cameraStartPosition;
            return;
        }

        // Read mouse movement to rotate the player and camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player horizontally
        transform.Rotate(0f, mouseX, 0f);

        // Limit how far the camera can look up or down
        angle = Mathf.Clamp(angle - mouseY, -60f, 60f);

        // Rotate the camera if one has been assigned
        if (playerCamera != null)
        {
            playerCamera.localEulerAngles = new Vector3(angle, 0f, 0f);
        }

        if(IsHidden){
            moveDirection = Vector3.zero;
            return;
        }

        // Read the movement keys
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate the movement direction relative to the player
        moveDirection = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;
        
        // Check whether either Shift key is being held
        bool sprintKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Check whether the player is moving
        bool isMoving = horizontalInput != 0 || verticalInput != 0;
        
        // Sprint only when Shift is held, the player is moving, and not exhausted
        if (sprintKey && isMoving && !isExhausted)
        {

            isSprinting = true;

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

            isSprinting = false;

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

        HandleFootsteps(isMoving);
    }

    // FixedUpdate is used for physics-related movement
    private void FixedUpdate()
    {
        if(rb.isKinematic){
            return;
        }

        // Calculate the player's movement velocity
        Vector3 velocity = moveDirection * moveSpeed;
        
        // Apply movement while keeping the existing vertical velocity
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    public void StartHiding(HideInteractable hideInteractable){
        if(!IsHidden && !IsTransitioning){
            StartCoroutine(StartRoutine(hideInteractable));
        }
    }

    public void StopHiding(){
        if(IsHidden && !IsTransitioning){
            StartCoroutine(ExitRoutine());
        }
    }

    private IEnumerator StartRoutine(HideInteractable hideInteractable){
        IsTransitioning = true;
        CurrentSpot = hideInteractable;
        hideInteractable.SetIsHiding(true);

        //Physics doesn't affect the player while hiding if they try to move
        moveDirection = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        playerCollider.enabled = false;

        //handle the the door animations of the locker and check if the player where they should hide and if they are hidden to the game engine
        hideInteractable.SetDoorOpen(true);
        yield return MoveTo(hideInteractable.HidingSpot.position, hideInteractable.HidingSpot.rotation, 0.4f);
        hideInteractable.SetDoorOpen(false);
        IsHidden = true;
        IsTransitioning = false;
    }

    private IEnumerator ExitRoutine(){
        IsTransitioning = true;
        HideInteractable hideInteractable = CurrentSpot;
        hideInteractable.SetDoorOpen(true);
        yield return MoveTo(CurrentSpot.ExitSpot.position, CurrentSpot.ExitSpot.rotation, 0.4f);
        hideInteractable.SetDoorOpen(false);
        rb.isKinematic = false;
        playerCollider.enabled = true;
        CurrentSpot.SetIsHiding(false);
        CurrentSpot = null;
        IsHidden = false;
        IsTransitioning = false;
    }

    private IEnumerator MoveTo(Vector3 spot, Quaternion rotate, float duration){
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        for(float time = 0f; time < 1f; time += Time.deltaTime / duration){
            transform.SetPositionAndRotation(Vector3.Lerp(startPosition, spot, time), Quaternion.Slerp(startRotation, rotate, time));
            yield return null;
        }
        transform.SetPositionAndRotation(spot, rotate);
        //Reset the camera pitch so that you constantly look straight between hiding and getting out of the hiding spot
        angle = 0f;
        if(playerCamera != null){
            playerCamera.localEulerAngles = Vector3.zero;
        }
    }

    // Plays footsteps and bobs the camera
    private void HandleFootsteps(bool isMoving)
    {
        // Standing still (reset the timer and move the camera back to normal)
        if (!isMoving)
        {
            stepTimer = 0f;
            playerCamera.localPosition = cameraStartPosition;

            // Stop the footstep sound
            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }

            return;
        }

        // Pick the settings for walking or sprinting
        float stepTime = isSprinting ? sprintStepTime : walkStepTime;
        float bobAmount = isSprinting ? sprintBobAmount : walkBobAmount;

        // Count up to the next step
        stepTimer = stepTimer + Time.deltaTime;

        if (stepTimer >= stepTime)
        {
            stepTimer = stepTimer - stepTime;

            // Play the footstep sound
            if (footstepSource != null)
            {
                footstepSource.pitch = isSprinting ? sprintPitch : 1f;
                footstepSource.Play();
            }
        }

        // The camera is lowest at the moment of each step
        float prog = stepTimer / stepTime;
        float bobYpos = -Mathf.Cos(prog * Mathf.PI * 2f) * bobAmount;
        playerCamera.localPosition = cameraStartPosition + new Vector3(0f, bobYpos, 0f);
    }
}
