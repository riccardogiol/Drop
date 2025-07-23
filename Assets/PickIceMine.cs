using System.Collections;
using UnityEngine;

public class PickIceMine : MonoBehaviour
{
    public int mineDamage = 2;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.GetComponent<EnemyHealth>().TakeDamage(mineDamage);
                StartCoroutine(animationAndDelayedDestroy());
                break;
            case "Flame":
                int otherEnergy = other.GetComponent<PickFlame>().energy;
                if (otherEnergy <= mineDamage)
                    other.GetComponent<PickFlame>().DestroyFlame();
                else
                {
                    other.GetComponent<PickFlame>().energy -= mineDamage;
                    other.GetComponent<PickFlame>().ScaleOnEnergy();
                }
                StartCoroutine(animationAndDelayedDestroy());
                break;
            case "Wall":
                Destroy(gameObject);
                break;
            case "IceMine":
                if (gameObject.GetInstanceID() > other.gameObject.GetInstanceID())
                    Destroy(gameObject);
                break;
        }
    }

    IEnumerator animationAndDelayedDestroy()
    {
        animator.SetTrigger("explode");
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
