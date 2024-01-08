using UnityEngine;

public class TileParticlesManager : MonoBehaviour
{
    public ParticleSystem smoke;
    public GameObject waterGrassPrefab;

    public void ActivateBurntParticle()
    {
        smoke.Play();
    }

    public void DesactivateBurntParticle()
    {
        smoke.Stop();
        GameObject wgpRef = Instantiate(waterGrassPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
        wgpRef.transform.parent = transform;
    }
}
