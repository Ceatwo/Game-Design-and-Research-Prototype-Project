using UnityEngine;

// Turns the camera up/down and the player body left/right with the mouse.
// Based on Brackeys' "FIRST PERSON MOVEMENT in Unity" tutorial.
// Goes on the Camera (child of the Player).
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 3f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Hide the mouse and keep it in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Press Escape to get the mouse back, click to lock it again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Mouse movement is already "per frame", so no Time.deltaTime here.
        // GetAxisRaw so there is no smoothing between the mouse and the camera.
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Look up and down (clamped so you can't flip over)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Turn the whole body left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}