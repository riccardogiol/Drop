using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    public bool isBurnt = true;
    public bool reactOnWater = false;

    public Animator decoAnimator;
    public ParticleSystem waterParticles;

    void Awake()
    {
        decoAnimator.SetBool("IsBurnt", isBurnt);
        return;
    }

    public void SetGreenSprite()
    {
        if (!isBurnt)
            return;
        isBurnt = false;
        if (waterParticles != null)
            waterParticles.Play();
        decoAnimator.SetBool("IsBurnt", isBurnt);
    }

    public void SetBurntSprite()
    {
        isBurnt = true;
        decoAnimator.SetBool("IsBurnt", isBurnt);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!reactOnWater)
            return;
        switch(other.tag)
        {
            case "Waterbullet":
            case "Wave":
                SetGreenSprite();
                break;
        }
    }

}
