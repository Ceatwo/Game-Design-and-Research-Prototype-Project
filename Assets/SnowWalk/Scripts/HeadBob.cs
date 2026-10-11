using UnityEngine;

// Moves the camera up and down in a wave while walking.
// Goes on the Camera.
public class HeadBob : MonoBehaviour
{
    public PlayerMovement player;

    public float bobSpeed = 10f;
    public float bobAmount = 0.05f;

    float defaultY;
    float timer;

    void Start()
    {
        // Remember where the camera started so we can return to it
        defaultY = transform.localPosition.y;
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;

        if (player.isMoving)
        {
            float speed = bobSpeed;
            if (player.isRunning)
            {
                speed = bobSpeed * 1.5f;
            }

            // Mathf.Sin goes smoothly between -1 and 1, which makes the up/down wave
            timer += Time.deltaTime * speed;
            pos.y = defaultY + Mathf.Sin(timer) * bobAmount;
        }
        else
        {
            // Slowly go back to normal height when we stop
            timer = 0f;
            pos.y = Mathf.Lerp(pos.y, defaultY, Time.deltaTime * 5f);
        }

        transform.localPosition = pos;
    }
}
