using UnityEngine;

// Plays a random footstep sound every so often while the player walks.
// Goes on the Player, with an AudioSource.
public class Footsteps : MonoBehaviour
{
    public PlayerMovement player;
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;

    // Time between steps in seconds
    public float walkStepTime = 0.5f;
    public float runStepTime = 0.32f;

    float stepTimer;

    void Update()
    {
        if (player.isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();

                if (player.isRunning)
                {
                    stepTimer = runStepTime;
                }
                else
                {
                    stepTimer = walkStepTime;
                }
            }
        }
        else
        {
            // Set to 0 so the first step plays straight away when we start walking
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepSounds.Length == 0)
        {
            return;
        }

        // Pick a random clip and change the pitch a little so it doesn't sound repetitive
        int index = Random.Range(0, footstepSounds.Length);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(footstepSounds[index]);
    }
}
