using UnityEngine;

public class BurntGrassGFXManager : MonoBehaviour
{
    public SpriteRenderer leafRenderer;
    public Sprite[] leafSprites;

    bool isVisible = false;
    bool isActive = false;
    
    public ParticleSystem petalBurstPrefab;

    void Awake()
    {
        if (Random.value < 0.7f)
            isVisible = true;
        leafRenderer.enabled = false;
        if (Random.value > 0.5f)
            leafRenderer.flipX = true;
        int index = Random.Range(0, leafSprites.Length);
        leafRenderer.sprite = leafSprites[index];
    }

    public void Activate()
    {
        if (isActive)
            return;
        if (isVisible)
            leafRenderer.enabled = true;
        isActive = true;

        petalBurstPrefab.Play();
    }

    public void Desactivate()
    {
        leafRenderer.enabled = false;
        isActive = false;
    }

}
