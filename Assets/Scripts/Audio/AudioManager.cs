using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    float volumeFactor = 0.5f;

    public Sound[] sounds;
    public Sound[] musics;

    public List<String> resetOnStart;

    float musicVolume, soundVolume;

    Transform playerPosition;

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

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        SetMusicVolume(musicVolume);
        SetSoundVolume(soundVolume);

        playerPosition = FindFirstObjectByType<PlayerHealth>().transform;
    }

    public void ResetSounds()
    {
        foreach (string s in resetOnStart)
            Stop(s);
    }

    public void Play(string name, Vector3 sourcePosition = new Vector3())
    {
        bool isMusics = false;
        if (sourcePosition != null && playerPosition == null)
            playerPosition = FindFirstObjectByType<PlayerHealth>().transform;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            s = Array.Find(musics, music => music.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound with name " + name + " not found");
                return;
            }
            isMusics = true;
        }

        if (s.source.isPlaying && s.source.loop)
        {
            if (s.distanceScaleVolume && sourcePosition != null)
            {
                float distance = Vector2.Distance(playerPosition.position, sourcePosition);
                if (isMusics)
                    s.source.volume = s.volume * volumeFactor * musicVolume * Math.Max(0, 1 - (distance / s.zeroVolumeDistance));
                else
                    s.source.volume = s.volume * volumeFactor * soundVolume * Math.Max(0, 1 - (distance / s.zeroVolumeDistance));
            }
            return;
        }

        if (s.pitchVariation)
            s.source.pitch = s.pitch * UnityEngine.Random.Range(0.7f, 1.3f);

        float volume = s.volume * volumeFactor;
        if (s.distanceScaleVolume && sourcePosition != null)
        {
            float distance = Vector2.Distance(playerPosition.position, sourcePosition);
            volume *= Math.Max(0, 1 - (distance / s.zeroVolumeDistance));
            if (volume <= 0)
                return;
        }

        if (s.loop)
        {
            if (isMusics)
                s.source.volume = volume * musicVolume;
            else
                s.source.volume = volume * soundVolume;
            s.source.Play();
        } else
            s.source.PlayOneShot(s.clip, volume);
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
