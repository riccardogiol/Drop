using System.Collections;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    Animator animator;
    public GameObject vaporBurst;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void PlayTriumph()
    {
        animator.SetTrigger("Triumph");
    }

    public void PlayEvaporation()
    {
        animator.SetTrigger("Evaporation");
        StartCoroutine(DelayedVaporBurst());
    }

    public void PlayShooting()
    {
        animator.SetTrigger("Shooting");
    }

    public void PlayCastingWave()
    {
        animator.SetTrigger("Wave");
    }

    IEnumerator DelayedVaporBurst()
    {
        yield return new WaitForSeconds(0.9f);
        Instantiate(vaporBurst, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
}
