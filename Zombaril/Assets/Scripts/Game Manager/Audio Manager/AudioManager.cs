using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;  // An array to hold all sound clips that can be played in the game.

    // Start is called before the first frame update
    void Start()
    {
            // Iterate over all sounds and add an AudioSource component for each, then set its clip and loop properties.
            foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }

        // Play the "Theme" sound when the AudioManager starts.
        PlaySound("Theme");
    }

    // Play a sound by name.
    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Play();
        }
    }

    // Stop a sound by name.
    public void StopSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Stop();
        }
    }
}

// A struct to hold information about a single sound clip.
[System.Serializable]
public class Sound
{
    public string name;  // The name of the sound clip.
    public AudioClip clip;  // The audio clip itself.
    [Range(0f, 1f)]
    public float volume = 1f;  // The volume at which the sound should be played.
    [Range(0.1f, 3f)]
    public float pitch = 1f;  // The pitch at which the sound should be played.
    public bool loop;  // Whether or not the sound should loop.
    [HideInInspector]
    public AudioSource source;  // The AudioSource component for the sound.
}