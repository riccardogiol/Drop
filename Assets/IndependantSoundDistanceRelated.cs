using System;
using UnityEngine;

public class IndependantSoundDistanceRelated : MonoBehaviour
{
    public string soundName;

    public float timer = 0.3f;
    float countdown = 0;

    Transform playerT;
    Sound sound;
    AudioSource audioSource;

    float baseVolume;

    public bool playOnStart = true;

    void Start()
    {
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null)
            playerT = FindFirstObjectByType<PlayerHealth>().transform;
        else
        {
            enabled = false;
            return;
        }
        sound = FindObjectOfType<AudioManager>().GetSound(soundName);
        if (sound == null)
        {
            enabled = false;
            return;
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        baseVolume = FindObjectOfType<AudioManager>().GetSoundVolumeFactor() * sound.volume;
        audioSource.clip = sound.source.clip;
        audioSource.volume = baseVolume;
        audioSource.pitch = sound.source.pitch;
        audioSource.loop = sound.source.loop;
        if (playOnStart)
            Play();
        AdjustVolumeOnDistance();
    }



    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            AdjustVolumeOnDistance();
            countdown = timer;
        }  
    }

    public void AdjustVolumeOnDistance()
    {
        float distance = Vector2.Distance(playerT.position, transform.position);
        audioSource.volume = baseVolume * Math.Max(0, 1 - (distance / sound.zeroVolumeDistance));
    }

    public void Play()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
