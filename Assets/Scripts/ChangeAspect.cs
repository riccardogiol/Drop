using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    public bool isBurnt = true;
    public bool reactOnWater = false;

    public Animator decoAnimator;
    public ParticleSystem waterParticles;
    public GameObject flowerStarter;

    void Awake()
    {
        decoAnimator.SetBool("IsBurnt", isBurnt);
    }

    void Start()
    {
        if (!isBurnt && flowerStarter!=null)
        {
            GameObject goref = Instantiate(flowerStarter, transform.position, Quaternion.identity);
            goref.GetComponent<TriggerFlowering>().enabled = true;
        }
    }

    public void SetGreenSprite()
    {
        if (!isBurnt)
            return;
        isBurnt = false;
        if (waterParticles != null)
            waterParticles.Play();
        if (flowerStarter != null)
            Instantiate(flowerStarter, transform.position, Quaternion.identity);
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
