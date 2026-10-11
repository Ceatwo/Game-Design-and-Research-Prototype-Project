using UnityEngine;

// Put one of these in each scene to choose its music and ambience.
// The names have to match the names in the AudioManager.
public class SceneMusic : MonoBehaviour
{
    public string musicName = "ForestMusic";
    public string ambienceName = "Wind";

    void Start()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogWarning("No AudioManager in the scene!");
            return;
        }

        AudioManager.instance.StopAllLooping();

        if (musicName != "")
        {
            AudioManager.instance.Play(musicName);
        }

        if (ambienceName != "")
        {
            AudioManager.instance.Play(ambienceName);
        }
    }
}
