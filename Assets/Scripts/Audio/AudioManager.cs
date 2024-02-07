using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public float volumeFactor = 1f;
    public Sound[] sounds;

    void Awake()
    {
        if (PlayerPrefs.GetInt("MusicState", 1) == 1)
            volumeFactor = 0.2f;
        else
            volumeFactor = 0f;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume*volumeFactor;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found");
            return;
        }
        if (s.source.isPlaying && s.source.loop)
            return;
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found");
            return;
        }
        s.source.Stop();
    }

    public void SetVolume(float value)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume*value;
        }
    }
}
