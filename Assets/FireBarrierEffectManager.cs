using UnityEngine;

public class FireBarrierEffectManager : MonoBehaviour
{
    public SpriteRenderer gfx;
    public ParticleSystem smoke;
    public ParticleSystem sparkles;
    public GameObject vaporBurstPrefab;

    Collider2D collider2D;

    void Awake()
    {
        collider2D = GetComponent<Collider2D>();  
    }

    public void Estinguish()
    {
        gfx.enabled = false;
        smoke.Stop();
        sparkles.Stop();
        Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);
        if (collider2D != null)
            collider2D.enabled = false;

    }
}
