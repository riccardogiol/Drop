using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public float volumeFactor = 0.2f;
    // dividere suoni e musica per gestire volumi separatamente
    public Sound[] sounds;
    public Sound[] musics;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * volumeFactor;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume * volumeFactor;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }

        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1));
        SetSoundVolume(PlayerPrefs.GetFloat("SoundVolume", 1));
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            s = Array.Find(musics, music => music.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound with name " + name + " not found");
                return;
            }
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
            s = Array.Find(musics, music => music.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound with name " + name + " not found");
                return;
            }
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
    
    public void SetMusicVolume(float value)
    {
        foreach (Sound m in musics)
        {
            m.source.volume = m.volume * value * volumeFactor;
        }
    }

    public void SetSoundVolume(float value)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * value * volumeFactor;
        }
    }
}
