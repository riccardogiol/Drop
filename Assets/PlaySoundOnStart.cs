using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    public string soundName = "FireBurst";
    public bool distanceRelated = false;

    void Start()
    {
        if (distanceRelated)
            FindObjectOfType<AudioManager>().Play(soundName, transform.position);
        else
            FindObjectOfType<AudioManager>().Play(soundName);
    }

    public void PlaySound(string codeSound)
    {
        //if (distance)
        //    FindObjectOfType<AudioManager>().Play(codeSound, transform.position);
        //else
            FindObjectOfType<AudioManager>().Play(codeSound);
    }
}
