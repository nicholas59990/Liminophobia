using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float sprintSpeed = 14f;
    [SerializeField] private float mouseSensitivity = 3.0f;

    [Header("Camera Settings")]
    [SerializeField] private Transform playerCamera;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private float angle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0f, mouseX, 0f);
        angle = Mathf.Clamp(angle - mouseY, -70f, 70f);
        if (playerCamera != null)
        {
            playerCamera.localEulerAngles = new Vector3(angle, 0f, 0f);
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        moveDirection = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = 7f;
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
}
