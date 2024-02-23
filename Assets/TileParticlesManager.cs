using UnityEngine;

public class TileParticlesManager : MonoBehaviour
{
    public ParticleSystem smoke;
    public GameObject waterGrassPrefab;
    public BurntGrassGFXManager burntGrassGFXManager;

    public void ActivateBurntParticle()
    {
        smoke.Play();
        burntGrassGFXManager.Activate();
    }

    public void DesactivateBurntParticle()
    {
        smoke.Stop();
        burntGrassGFXManager.Desactivate();
        GameObject wgpRef = Instantiate(waterGrassPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
        wgpRef.transform.parent = transform;
    }
}
