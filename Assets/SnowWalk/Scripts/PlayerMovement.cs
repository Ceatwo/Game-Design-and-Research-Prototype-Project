using UnityEngine;

// Moves the player with WASD using a CharacterController.
// Based on Brackeys' "FIRST PERSON MOVEMENT in Unity" tutorial.
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float walkSpeed = 2.5f;
    public float runSpeed = 4.5f;
    public float gravity = -9.81f;

    // An empty object at the player's feet, used to check if we are on the ground
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Other scripts (HeadBob, Footsteps) read these
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool isRunning;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // Check if there is ground in a small sphere under the player
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset falling speed when on the ground (-2 keeps us pressed onto slopes)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // WASD / arrow keys
        // GetAxisRaw gives -1, 0 or 1 straight away. GetAxis smooths it,
        // which makes the player slide and feel like it's dragging.
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Move relative to the way the player is facing
        Vector3 move = transform.right * x + transform.forward * z;

        // Stop diagonal movement being faster than straight movement
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Hold shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = walkSpeed;
        if (isRunning)
        {
            speed = runSpeed;
        }

        controller.Move(move * speed * Time.deltaTime);

        isMoving = isGrounded && move.magnitude > 0.1f;

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}