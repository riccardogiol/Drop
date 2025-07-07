using System.Collections;
using UnityEngine;

public class TileParticlesManager : MonoBehaviour
{
    public ParticleSystem smoke;
    public GameObject waterGrassPrefab;
    public BurntGrassGFXManager burntGrassGFXManager;
    bool isBurnt = false;

    public void ActivateBurntParticle()
    {
        isBurnt = true;
        StartCoroutine(DelayedPlay(smoke, Random.value));
        burntGrassGFXManager.Activate();
    }

    public void DesactivateBurntParticle()
    {
        isBurnt = false;
        smoke.Stop();
        burntGrassGFXManager.Desactivate();
        GameObject wgpRef = Instantiate(waterGrassPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
        wgpRef.transform.parent = transform;
    }

    IEnumerator DelayedPlay(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isBurnt)
            ps.Play();
    }
}
