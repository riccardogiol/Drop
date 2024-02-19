using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChangeAspect : MonoBehaviour
{
    public bool isBurnt = true;
    public bool reactOnWater = false;

    public Animator decoAnimator;
    public ParticleSystem waterParticles;
    public ParticleSystem leavesParticles;
    public ParticleSystem burntLeavesParticles;
    public GameObject flowerStarter;
    public List<int2> touchingCellsCoordinates;
    List<Vector3> touchingCells;
    PlaygroundManager playgroundManager;

    void Awake()
    {
        decoAnimator.SetBool("IsBurnt", isBurnt);
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        touchingCells = new List<Vector3>();
        foreach(int2 point in touchingCellsCoordinates)
            touchingCells.Add(transform.position + new Vector3(point.x + 0.5f, point.y + 0.5f));
    }

    void Start()
    {
        if (!isBurnt && flowerStarter!=null)
        {
            GameObject goref = Instantiate(flowerStarter, transform.position, Quaternion.identity);
            goref.GetComponent<TriggerFlowering>().enabled = true;
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
        if (flowerStarter != null)
            Instantiate(flowerStarter, transform.position, Quaternion.identity);
        decoAnimator.SetBool("IsBurnt", isBurnt);

        if (playgroundManager != null)
        {
            foreach(Vector3 point in touchingCells)
                playgroundManager.WaterOnPosition(point);
        }
    }

    public void SetBurntSprite(bool playLeaves = true)
    {
        isBurnt = true;
        decoAnimator.SetBool("IsBurnt", isBurnt);
        if (burntLeavesParticles != null && playLeaves)
            burntLeavesParticles.Play();
        if (playgroundManager != null)
        {
            foreach(Vector3 point in touchingCells)
                playgroundManager.FireOnPosition(point);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!reactOnWater)
            return;
        switch(other.tag)
        {
            case "Waterbullet":
            case "Wave":
                SetGreenSprite();
                break;
        }
    }

    public void FlipX()
    {
        GameObject auxGO = transform.Find("GFX").gameObject;
        if (auxGO != null)
            auxGO.GetComponent<SpriteRenderer>().flipX = true;
    }

}
