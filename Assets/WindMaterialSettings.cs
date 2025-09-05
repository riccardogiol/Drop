using UnityEngine;

public class WindMaterialSettings : MonoBehaviour
{
    public Material windMaterial;

    public float windIntensity = 0.2f;
    public float brightness = 1.0f;

    public bool hueShifting = false;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;
        if (windMaterial == null)
            return;
        spriteRenderer.material = new Material(windMaterial);

        if (hueShifting)
        {
            float shifting = Random.Range(-0.05f, 0.05f);
            if (shifting > 0)
                spriteRenderer.material.SetFloat("_Hue", shifting);
            else
                spriteRenderer.material.SetFloat("_Hue", 1 + shifting);
        }
        spriteRenderer.material.SetFloat("_Brightness", brightness);
        spriteRenderer.material.SetFloat("_WindIntensity", windIntensity);
    }

}
