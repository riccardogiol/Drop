using UnityEngine;

public class ChangeWindIntensity : MonoBehaviour
{
    public Material colorAdjustmentMaterial;
    public float windIntensity = 1.0f;
    public float bloomIntensity = 0;

    void Start()
    {
        SpriteRenderer spriteRenderer= GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(colorAdjustmentMaterial);
        spriteRenderer.material.SetFloat("_WindIntensity", windIntensity);
        if (bloomIntensity > 0f)
                spriteRenderer.material.SetColor("_Color", new Color(1+bloomIntensity, 1+bloomIntensity, 1+bloomIntensity));
    }
}
