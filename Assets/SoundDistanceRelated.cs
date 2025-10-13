using UnityEngine;

public class SoundDistanceRelated : MonoBehaviour
{
    ChangeAspect changeAspect;
    public string soundName;

    float timer = 0.3f;
    float countdown = 0;

    void Awake()
    {
        changeAspect = GetComponent<ChangeAspect>();  
    }

    void Start()
    {
        PlaySoundDistanceRelated();
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            PlaySoundDistanceRelated();
            countdown = timer;
        }  
    }

    void PlaySoundDistanceRelated()
    {
        if (changeAspect.isBurnt)
            FindObjectOfType<AudioManager>().Stop(soundName);
        else
            FindObjectOfType<AudioManager>().Play(soundName, transform.position);
    }
}
