using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float topClamp = 90f;
    public float bottomClamp = -90f;
    void Start()
    {
        // locking the cursor to the middle of the screen  and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //getting mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotation  around  the x axis (look up and down)
        xRotation -= mouseY;

        //clamp to rotation
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);
        // rotation  around  the y axis (look left and right)
        yRotation += mouseX;

        //apply rotations to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);


    }
}
