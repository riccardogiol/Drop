
using UnityEngine;

public class SetBloomIntensity : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Material colorAdjustmentMaterial;

    public float bloomIntensity = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(colorAdjustmentMaterial);
        Color whiteColor = new Color(1, 1, 1);
        Color hdrColor = whiteColor * bloomIntensity;
        spriteRenderer.material.EnableKeyword("_EMISSION");
        spriteRenderer.material.SetColor("_Color", hdrColor);
    }
}
