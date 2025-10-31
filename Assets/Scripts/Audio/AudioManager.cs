using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    float volumeFactor = 0.8f;

    public Sound[] sounds;
    public Sound[] voices;
    public Sound[] musics;
    public AudioLowPassFilter lowPassFilter;

    public List<string> resetOnStart;

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

        foreach (Sound s in voices)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * volumeFactor;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound m in musics)
        {
            m.source = lowPassFilter.gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume * volumeFactor;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1);
        SetMusicVolume(musicVolume);
        SetSoundVolume(soundVolume);

        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null)
            playerPosition = FindFirstObjectByType<PlayerHealth>().transform;
    }

    public void ResetSounds()
    {
        foreach (string s in resetOnStart)
            Stop(s, false);
    }

    public void Play(string name, Vector3 sourcePosition = new Vector3())
    {
        bool isMusics = false;
        if (sourcePosition != null && playerPosition == null)
        {
            PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
            if (player != null)
                playerPosition = FindFirstObjectByType<PlayerHealth>().transform;
        }

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
            if (s.distanceScaleVolume && sourcePosition != null && playerPosition != null)
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
        if (s.distanceScaleVolume && sourcePosition != null && playerPosition != null)
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

            if (s.fading)
                Fade(s, 0, s.source.volume);
            else
                s.source.Play();
        }
        else
            s.source.PlayOneShot(s.clip, volume);
    }
    
    public void PlayVoice(string code)
    {
        if (UnityEngine.Random.value > 0.3)
            return;
        Sound[] vs = Array.FindAll(voices, voice => voice.name == code);
        vs[UnityEngine.Random.Range(0, vs.Length)].source.Play();
    }

    public void Stop(string name, bool fadingAllowed = true)
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

        if (s.fading && fadingAllowed)
            Fade(s, s.source.volume, 0);
        else
            s.source.Stop();
    }

    void Fade(Sound s, float startVolume, float endVolume, float time = 3.0f)
    {
        StartCoroutine(Fading(s, startVolume, endVolume, time));
    }

    IEnumerator Fading(Sound s, float startVolume, float endVolume, float time = 1.0f)
    {
        float countdown = time;
        s.source.volume = startVolume;
        if (startVolume == 0)
            s.source.Play();
        while (countdown >= 0)
        {
            countdown -= Time.deltaTime; //??
            s.source.volume = Mathf.Lerp(startVolume, endVolume, (time - countdown) / time);
            yield return null;
        }
        if (endVolume == 0)
            s.source.Stop();
        else
            s.source.volume = endVolume;
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            s = Array.Find(musics, music => music.name == name);
        return s;
    }
    
    public float GetSoundVolumeFactor()
    {
        return volumeFactor * soundVolume;
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
        musicVolume = value;
        foreach (Sound m in musics)
        {
            m.source.volume = m.volume * value * volumeFactor;
        }
    }

    public void SetSoundVolume(float value)
    {
        soundVolume = value;
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * value * volumeFactor;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code">1 Main, 2 Rock, 3 Soft</param>
    public void StartStageMusic(int code = 1)
    {
        string mainMusicName = "StageMusic";
        string SoftMusicName = "StageMusicSoft";
        string RockMusicName = "StageMusicRock";

        Sound mainMusic = Array.Find(musics, music => music.name == mainMusicName);
        if (mainMusic == null)
            return;
        Sound softMusic = Array.Find(musics, music => music.name == SoftMusicName);
        if (softMusic == null)
            return;
        Sound rockMusic = Array.Find(musics, music => music.name == RockMusicName);
        if (rockMusic == null)
            return;

        float mmvolume = mainMusic.volume * volumeFactor * musicVolume;
        float rvolume = rockMusic.volume * volumeFactor * musicVolume;
        float svolume = softMusic.volume * volumeFactor * musicVolume;

        if (code == 1)
        {
            mainMusic.source.volume = mmvolume;
            rockMusic.source.volume = 0;
            softMusic.source.volume = 0;
        }
        else if (code == 2)
        {
            mainMusic.source.volume = 0;
            rockMusic.source.volume = rvolume;
            softMusic.source.volume = 0;
        } else
        {
            mainMusic.source.volume = 0;
            rockMusic.source.volume = 0;
            softMusic.source.volume = svolume;
        }

        if (mainMusic.source.isPlaying)
            return;
        
        mainMusic.source.Play();
        rockMusic.source.Play();
        softMusic.source.Play();
    }

    public void StartBossMusic()
    {
        string musicName = "StageMusicBoss";

        Sound music = Array.Find(musics, music => music.name == musicName);
        if (music == null)
            return;

        float volume = music.volume * volumeFactor * musicVolume;

        music.source.volume = volume;
    
        if (music.source.isPlaying)
            return;
        music.source.Play();
    }
    
    public void StopStageMusic()
    {
        string mainMusicName = "StageMusic";
        string SoftMusicName = "StageMusicSoft";
        string RockMusicName = "StageMusicRock";
        string BossMusicName = "StageMusicBoss";

        Sound mainMusic = Array.Find(musics, music => music.name == mainMusicName);
        if (mainMusic == null)
            return;
        Sound softMusic = Array.Find(musics, music => music.name == SoftMusicName);
        if (softMusic == null)
            return;
        Sound rockMusic = Array.Find(musics, music => music.name == RockMusicName);
        if (rockMusic == null)
            return;
        Sound bossMusic = Array.Find(musics, music => music.name == BossMusicName);
        if (bossMusic == null)
            return;
        
        mainMusic.source.Stop();
        softMusic.source.Stop();
        rockMusic.source.Stop();
        bossMusic.source.Stop();
    }

    public void LowFilerEnter()
    {
        lowPassFilter.cutoffFrequency = 500;
    }

    public void LowFilerExit()
    {
        lowPassFilter.cutoffFrequency = 22000;
    }
    
    public void MusicSpeedDown()
    {
        string mainMusicName = "StageMusic";
        string SoftMusicName = "StageMusicSoft";
        string RockMusicName = "StageMusicRock";
        string BossMusicName = "StageMusicBoss";

        Sound mainMusic = Array.Find(musics, music => music.name == mainMusicName);
        if (mainMusic == null)
            return;
        Sound softMusic = Array.Find(musics, music => music.name == SoftMusicName);
        if (softMusic == null)
            return;
        Sound rockMusic = Array.Find(musics, music => music.name == RockMusicName);
        if (rockMusic == null)
            return; 
        Sound bossMusic = Array.Find(musics, music => music.name == BossMusicName);
        if (bossMusic == null)
            return;

        mainMusic.source.pitch = 0.9f;
        rockMusic.source.pitch = 0.9f;
        softMusic.source.pitch = 0.9f;
        bossMusic.source.pitch = 0.9f;
    }
    
    public void MusicSpeedRestore()
    {
        string mainMusicName = "StageMusic";
        string SoftMusicName = "StageMusicSoft";
        string RockMusicName = "StageMusicRock";
        string BossMusicName = "StageMusicBoss";

        Sound mainMusic = Array.Find(musics, music => music.name == mainMusicName);
        if (mainMusic == null)
            return;
        Sound softMusic = Array.Find(musics, music => music.name == SoftMusicName);
        if (softMusic == null)
            return;
        Sound rockMusic = Array.Find(musics, music => music.name == RockMusicName);
        if (rockMusic == null)
            return; 
        Sound bossMusic = Array.Find(musics, music => music.name == BossMusicName);
        if (bossMusic == null)
            return;

        mainMusic.source.pitch = 1f;
        rockMusic.source.pitch = 1f;
        softMusic.source.pitch = 1f;
        bossMusic.source.pitch = 1f;
    }
}
