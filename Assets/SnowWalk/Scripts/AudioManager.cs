using UnityEngine;

// Holds the settings for one sound. [System.Serializable] makes it show up in the Inspector.
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

// Plays music, ambience and sound effects by name, e.g. AudioManager.instance.Play("Wind");
// Based on Brackeys' "Introduction to AUDIO in Unity" tutorial.
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        // Only allow one AudioManager. If we load a scene that has another one, delete the new one.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Keep this object when changing scenes (so music doesn't cut out)
        DontDestroyOnLoad(gameObject);

        // Make an AudioSource for every sound in the list
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            return;
        }

        // Don't restart looping sounds (music/wind) if they are already playing
        if (s.loop && s.source.isPlaying)
        {
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    // Stops all music and ambience, used when changing scenes
    public void StopAllLooping()
    {
        foreach (Sound s in sounds)
        {
            if (s.loop)
            {
                s.source.Stop();
            }
        }
    }

    Sound FindSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                return s;
            }
        }

        Debug.LogWarning("Sound: " + name + " not found!");
        return null;
    }
}
