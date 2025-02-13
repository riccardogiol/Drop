using UnityEngine;

public class DecorationAnimationManager : MonoBehaviour
{
    Animator decoAnimator;

    bool isGreen = false;

    bool multyAnimation = false;
    public bool anim1 = false;
    public float anim1PercAppearance = 1;
    public bool anim2 = false;
    public float anim2PercAppearance = 1;
    public float idlePercAppearance = 1;

    public float timer = 1.0f;
    float countdown = 0;

    void Awake()
    {
        decoAnimator = GetComponent<Animator>();
        float totalPerc = 0;
        if (anim1)
        {
            multyAnimation = true;
            totalPerc += anim1PercAppearance;
            totalPerc += idlePercAppearance;
        }
        if (anim2)
        {
            totalPerc += anim2PercAppearance;
        }
        if (multyAnimation)
        {
            idlePercAppearance /= totalPerc;
            anim1PercAppearance /= totalPerc;
            anim2PercAppearance /= totalPerc;
        }
    }

    void Update()
    {
        if (isGreen && multyAnimation)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0)
            {
                countdown = timer;
                float randVal = Random.value;
                if (randVal < anim1PercAppearance)
                    decoAnimator.SetTrigger("Anim1");
                else if (anim2 && randVal < (anim2PercAppearance + anim1PercAppearance))
                    decoAnimator.SetTrigger("Anim2");
            }
        }
    }


    public void SetGreen()
    {
        decoAnimator.SetBool("IsBurnt", false);
        isGreen = true;
        countdown = timer;
    }

    public void SetBurnt()
    {
        decoAnimator.SetBool("IsBurnt", true);
        isGreen = false;
    }
}
