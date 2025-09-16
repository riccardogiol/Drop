
using UnityEngine;

public class SetBloomIntensity : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    ParticleSystem ps;

    public Material colorAdjustmentMaterial;

    public float bloomIntensity = 0f;

    void Awake()
    {
        Color whiteColor = new Color(1, 1, 1);
        Color hdrColor = whiteColor * bloomIntensity;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            ps = GetComponent<ParticleSystem>();
            ps.GetComponent<ParticleSystemRenderer>().material = new Material(colorAdjustmentMaterial);
            ps.GetComponent<ParticleSystemRenderer>().material.EnableKeyword("_EMISSION");
            ps.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", hdrColor);
            return;
        }
        
        spriteRenderer.material = new Material(colorAdjustmentMaterial);
        spriteRenderer.material.EnableKeyword("_EMISSION");
        spriteRenderer.material.SetColor("_Color", hdrColor);
    }
}
