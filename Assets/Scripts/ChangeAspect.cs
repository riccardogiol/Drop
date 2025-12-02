using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    [Header("Initial parameters")]
    public bool isBurnt = true;
    public bool reactOnWater = false;

    [Header("Set Sprites or Animator")]
    public Sprite greenSprite;
    public Sprite burntSprite;
    public DecorationAnimationManager decoAnimator;
    
    [Header("Fixed Parameters")]
    public ParticleSystem waterParticles;
    public ParticleSystem leavesParticles;
    public ParticleSystem burntLeavesParticles;
    public GameObject flowerStarter;
    GameObject flowerStarterGO;
    public GameObject fireBarrier;
    public List<int2> touchingCellsCoordinates;
    List<Vector3> touchingCells;
    PlaygroundManager playgroundManager;
    public SpriteRenderer spriteRenderer;

    public Material colorAdjustmentMaterial;

    public float bloomIntensity = 0f;

    void Awake()
    {
        if (spriteRenderer == null)
        {
            GameObject auxGO = transform.Find("GFX").gameObject;
            if (auxGO != null)
                spriteRenderer = auxGO.GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            if (isBurnt)
                spriteRenderer.sprite = burntSprite;
            else
                spriteRenderer.sprite = greenSprite;
        }

        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        touchingCells = new List<Vector3>();
        foreach(int2 point in touchingCellsCoordinates)
            touchingCells.Add(transform.position + new Vector3(point.x + 0.5f, point.y + 0.5f));
        
        if (colorAdjustmentMaterial != null)
        {
            spriteRenderer.material = new Material(colorAdjustmentMaterial);
            if (bloomIntensity > 0f)
                spriteRenderer.material.SetColor("_Color", new Color(1+bloomIntensity, 1+bloomIntensity, 1+bloomIntensity)*bloomIntensity);
        }

        if (fireBarrier != null)
        {
            Material newMat = new Material(spriteRenderer.sharedMaterial);
            spriteRenderer.material = newMat;
            spriteRenderer.material.SetColor("_Color", new Color(0.4f, 0.4f, 0.4f));
        }
    }

    void Start()
    {
        if (decoAnimator != null)
        {
            if (isBurnt)
                decoAnimator.SetBurnt();
            else
                decoAnimator.SetGreen();
        }
        if (flowerStarter!=null)
        {
            flowerStarterGO = Instantiate(flowerStarter, transform.position, Quaternion.identity);
            flowerStarterGO.transform.parent = transform;
            if (isBurnt)
               flowerStarterGO.SetActive(false);
            else
                flowerStarterGO.SetActive(true);
        }
    }

    public void SetGreenSprite(bool playWater = true, bool playLeaves = true)
    {
        if (!isBurnt)
            return;
        isBurnt = false;
        if (waterParticles != null && playWater)
            waterParticles.Play();
        if (leavesParticles != null && playLeaves)
            leavesParticles.Play();
        if (flowerStarter != null && flowerStarterGO != null)
            flowerStarterGO.SetActive(true);
        if (fireBarrier != null)
        {
            fireBarrier.GetComponent<FireBarrierEffectManager>().Estinguish();
            Material newMat = new Material(spriteRenderer.sharedMaterial);
            spriteRenderer.material = newMat;
            spriteRenderer.material.SetColor("_Color", new Color(1, 1, 1));
        }

        if (decoAnimator != null)
            decoAnimator.SetGreen();
        else if (spriteRenderer != null)
            spriteRenderer.sprite = greenSprite;

        if (reactOnWater && playgroundManager != null)
        {
            foreach(Vector3 point in touchingCells)
                playgroundManager.WaterOnPosition(point);
        }
    }

    public void SetBurntSprite(bool playLeaves = true)
    {
        isBurnt = true;
        if (decoAnimator != null)
            decoAnimator.SetBurnt();
        else if (spriteRenderer != null)
            spriteRenderer.sprite = burntSprite;

        if (burntLeavesParticles != null && playLeaves)
            burntLeavesParticles.Play();
        if (flowerStarter != null)
            flowerStarterGO.SetActive(false);
        if (reactOnWater && playgroundManager != null)
        {
            foreach(Vector3 point in touchingCells)
                playgroundManager.FireOnPosition(point);
        }
    }

    public void FlipX()
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = true;
    }

    public void ColorAdjustment(float hue, float brightness, bool noiseOffset = true)
    {
        if (spriteRenderer == null)
            return;
        if (colorAdjustmentMaterial != null)
        {
            if (hue < 0)
                spriteRenderer.material.SetFloat("_Hue", 1 + hue);
            else
                spriteRenderer.material.SetFloat("_Hue", hue);
            spriteRenderer.material.SetFloat("_Brightness", brightness);
            if (noiseOffset)
                spriteRenderer.material.SetFloat("_NoiseOffset", UnityEngine.Random.Range(0, 10f));
        } else {
            spriteRenderer.color = new Color(brightness, brightness, brightness);
        }
    }

    public void SetTransparancy(bool transparent)
    {
        if (transparent)
            spriteRenderer.material.SetInt("_HalfTransparent", 1);
        else
            spriteRenderer.material.SetInt("_HalfTransparent", 0);
    }

}
