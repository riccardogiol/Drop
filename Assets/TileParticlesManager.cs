using UnityEngine;

public class TileParticlesManager : MonoBehaviour
{
    public ParticleSystem smoke;

    public void ActivateBurntParticle()
    {
        smoke.Play();
    }

    public void DesactivateBurntParticle()
    {
        smoke.Stop();
    }
}
