using UnityEngine;
using UnityEngine.SceneManagement;

// Loads another scene when the player walks into this trigger (e.g. the house door).
// Needs a Collider with "Is Trigger" ticked. The player must be tagged "Player".
public class SceneDoor : MonoBehaviour
{
    public string sceneToLoad = "House";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
