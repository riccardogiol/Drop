using UnityEngine;

public class SetWindIntensity : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Material colorAdjustmentMaterial;

    public float windIntensity = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(colorAdjustmentMaterial);
        spriteRenderer.material.SetFloat("_WindIntensity", windIntensity);
    }
}
