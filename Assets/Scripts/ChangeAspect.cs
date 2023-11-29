using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    public bool isBurnt = true;

    public Animator decoAnimator;

    void Awake()
    {
        decoAnimator.SetBool("IsBurnt", isBurnt);
        return;
    }

    public void SetGreenSprite()
    {
        isBurnt = false;
        decoAnimator.SetBool("IsBurnt", isBurnt);
    }

    public void SetBurntSprite()
    {
        isBurnt = true;
        decoAnimator.SetBool("IsBurnt", isBurnt);
    }
}
