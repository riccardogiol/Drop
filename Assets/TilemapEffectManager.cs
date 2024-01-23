using System.Collections.Generic;
using UnityEngine;

public class TilemapEffectManager : MonoBehaviour
{
    GameObject particleCollider;
    List<TileFlowerManager> flowerTiles;
    RuleTileStateManager ruleTileStateManager;

    void Start()
    {
        if (particleCollider == null)
            particleCollider = Resources.Load<GameObject>("ParticleCollider");
        ruleTileStateManager = GetComponent<RuleTileStateManager>();
        flowerTiles = new List<TileFlowerManager>();
    }

    public void SpawnParticleColliders(int maxX, int maxY)
    {
        if (particleCollider == null)
            return;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                RuleTile currentTile = ruleTileStateManager.GetTile(new Vector3Int(x, y, 0));
                if (currentTile != null)
                {
                    if (ruleTileStateManager.IsTileBurnt(currentTile))
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), true); 
                    else
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), false);   
                }
            }
        }
    }
    
    void SpawnParticleCollider(Vector3 position, bool isBurning)
    {
        GameObject goRef = Instantiate(particleCollider, position, Quaternion.identity);
        goRef.transform.parent = transform;
        if (isBurning)
            goRef.GetComponent<TileParticlesManager>().ActivateBurntParticle();
        goRef.GetComponent<TileFlowerManager>().tilemapEffectManager = this;
    }

    public void CollectFlowerTiles()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("ParticleCollider"))
            {
                TileFlowerManager tfmAux = child.GetComponent<TileFlowerManager>();
                if (!tfmAux.isObstacle)
                {
                    flowerTiles.Add(tfmAux);
                }
            }
        }
    }

    public void SetFlowerSpreading(float spreadInterval)
    {
        foreach (TileFlowerManager tfm in flowerTiles)
        {
            tfm.checkSpreadingEligibilityInterval = spreadInterval;
        }
    }


    public void BurnTile(Vector3 position)
    {
        GameObject particleCollider = GetParticleCollider(position);
        if (particleCollider != null)
        {
            particleCollider.GetComponent<TileParticlesManager>().ActivateBurntParticle();
            StopFloweringAllAround(position);
        }
    }

    public void WaterTile(Vector3 position)
    {
        GameObject particleCollider = GetParticleCollider(position);
        if (particleCollider != null)
            particleCollider.GetComponent<TileParticlesManager>().DesactivateBurntParticle();
    }

    public GameObject GetParticleCollider(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("ParticleCollider"))
                return item.gameObject;
        }
        return null;
    }

    public bool IsTileBurnt(Vector3 pos)
    {
        return ruleTileStateManager.IsTileBurnt(pos);
    }

    void StopFloweringAllAround(Vector3 onCellPoint)
    {
        for (float y = onCellPoint.y - 1; y <= onCellPoint.y + 1; y++)
        {
            for (float x = onCellPoint.x - 1; x <= onCellPoint.x + 1; x++)
            {
                GameObject goRef = GetParticleCollider(new Vector3(x, y));
                if (goRef != null)
                    goRef.GetComponent<TileFlowerManager>().StopFlowering();
            }
        }
    }
}
