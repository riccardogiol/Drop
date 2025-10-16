using UnityEngine;

public class SoundDistanceRelated : MonoBehaviour
{
    ChangeAspect changeAspect;
    public string soundName;

    public bool onChangeAspect = true;

    public float timer = 0.3f;
    float countdown = 0;

    public bool play;

    void Awake()
    {
        if (onChangeAspect)
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

    public void PlaySoundDistanceRelated()
    {
        if (onChangeAspect)
            play = !changeAspect.isBurnt;

        if (play)
            FindObjectOfType<AudioManager>().Play(soundName, transform.position);
        else
            FindObjectOfType<AudioManager>().Stop(soundName);
    }
}
