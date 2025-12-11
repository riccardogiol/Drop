using System.Collections;
using UnityEngine;

public class TileParticlesManager : MonoBehaviour
{
    public ParticleSystem smoke;
    public GameObject waterGrassPrefab;
    public BurntGrassGFXManager burntGrassGFXManager;
    public GameObject cellHighlighterGFX;
    bool isBurnt = false;

    public void ActivateBurntParticle()
    {
        isBurnt = true;
        StartCoroutine(DelayedPlay(smoke, Random.value));
        burntGrassGFXManager.Activate();
    }

    public void ActivateHighlighter()
    {
        if (cellHighlighterGFX != null)
            cellHighlighterGFX.SetActive(true);
    }

     public void DesctivateHighlighter()
    {
        if (cellHighlighterGFX != null)
            cellHighlighterGFX.SetActive(false);
    }

    public void DesactivateBurntParticle()
    {
        isBurnt = false;
        smoke.Stop();
        burntGrassGFXManager.Desactivate();
        if (cellHighlighterGFX != null)
            cellHighlighterGFX.SetActive(false);
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
