using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;
    private float xRotation;
    private float yRotation;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

     public void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
