using UnityEngine;

public class FireBarrierEffectManager : MonoBehaviour
{
    public SpriteRenderer gfx;
    public ParticleSystem smoke;
    public ParticleSystem sparkles;
    public GameObject vaporBurstPrefab;

    public void Estinguish()
    {
        gfx.enabled = false;
        smoke.Stop();
        sparkles.Stop();
        Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);

    }
}
