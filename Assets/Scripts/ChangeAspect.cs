using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    public bool isBurnt = true;

    public Animator decoAnimator;
    public ParticleSystem waterParticles;

    void Awake()
    {
        decoAnimator.SetBool("IsBurnt", isBurnt);
        return;
    }

    public void SetGreenSprite()
    {
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
}
