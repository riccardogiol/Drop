using UnityEngine;

public class TilemapWallEffectManager : MonoBehaviour
{
    public GameObject wallParticleCollider;
    //List<TileFlowerManager> flowerTiles;
    RuleTileStateManager ruleTileStateManager;
    bool flowersCollected = false;
    
    public Sprite[] leafList;

    void Awake()
    {
        if ( wallParticleCollider == null)
             wallParticleCollider = Resources.Load<GameObject>("WallParticleCollider");
        ruleTileStateManager = GetComponent<RuleTileStateManager>();
        flowersCollected = false;
    }

    public void SpawnParticleColliders(int maxX, int maxY)
    {
        if (wallParticleCollider == null)
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
        GameObject goRef = Instantiate(wallParticleCollider, position, Quaternion.identity);
        goRef.transform.parent = transform;
        if (isBurning)
            goRef.GetComponent<LeavesGFXManager>().Uproot();
        else
            goRef.GetComponent<LeavesGFXManager>().Plant();
    }

    // void InitializeFlowerGrafic(TileFlowerManager tfm)
    // {
    //     int indexLvl1 = Random.Range(0, petalListLvl1.Length);
    //     int indexLvl2 = Random.Range(0, petalListLvl2.Length);
    //     int indexLvl3 = Random.Range(0, petalListLvl3.Length);

    //     FlowerGFXData fgd = new FlowerGFXData(
    //         petalListLvl1[indexLvl1], leafListLvl1[indexLvl1],
    //         petalListLvl2[indexLvl2], leafListLvl2[indexLvl2],
    //         petalListLvl3[indexLvl3], leafListLvl3[indexLvl3],
    //         colors[Random.Range(0, colors.Length)]);
    //     tfm.SetFlowerGFX(fgd);
    // }

    public void BurnTile(Vector3 position)
    {
        GameObject particleCollider = GetParticleCollider(position);
        if (particleCollider != null)
            particleCollider.GetComponent<LeavesGFXManager>().Uproot();
    }

    public void WaterTile(Vector3 position)
    {
        GameObject particleCollider = GetParticleCollider(position);
        if (particleCollider != null)
            particleCollider.GetComponent<LeavesGFXManager>().Plant();
    }

    public GameObject GetParticleCollider(Vector3 onCellPoint)
    {
        Collider2D[] results = Physics2D.OverlapPointAll(onCellPoint);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("WallParticleCollider"))
                return item.gameObject;
        }
        return null;
    }

    public bool IsTileBurnt(Vector3 pos)
    {
        return ruleTileStateManager.IsTileBurnt(pos);
    }
}
