using UnityEngine;

public class TilemapWallEffectManager : MonoBehaviour
{
    public GameObject wallParticleCollider;
    public GameObject wallParticleCollider2;
    RuleTileStateManager ruleTileStateManager;
    
    void Awake()
    {
        if ( wallParticleCollider == null)
             wallParticleCollider = Resources.Load<GameObject>("WallParticleCollider");
        ruleTileStateManager = GetComponent<RuleTileStateManager>();
    }

    public void SpawnParticleColliders(int maxX, int maxY)
    {
        if (wallParticleCollider == null)
            return;
        bool alternativeTile = false;
        RuleTile currentTile;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                currentTile = ruleTileStateManager.GetTile(new Vector3Int(x, y, 0));
                if (currentTile != null)
                {
                    alternativeTile = ruleTileStateManager.IsAlternativeTile(currentTile);
                    if (ruleTileStateManager.IsTileBurnt(currentTile))
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), true, alternativeTile);
                    else
                        SpawnParticleCollider(new Vector3(x + 0.5f, y + 0.5f), false, alternativeTile);   
                }
            }
        }
    }
    
    void SpawnParticleCollider(Vector3 position, bool isBurning, bool alternativeTile = false)
    {
        GameObject goRef;
        if (alternativeTile && wallParticleCollider2 != null)
            goRef = Instantiate(wallParticleCollider2, position, Quaternion.identity);
        else 
            goRef = Instantiate(wallParticleCollider, position, Quaternion.identity);
        goRef.transform.parent = transform;
        if (isBurning)
            goRef.GetComponent<LeavesGFXManager>().Uproot();
        else
            goRef.GetComponent<LeavesGFXManager>().Plant();
    }

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
