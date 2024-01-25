using System.Collections;
using UnityEngine;

public class TileFlowerManager : MonoBehaviour
{
    public bool isFlowering = false;
    public bool isObstacle = false;

    public FlowerGFXManager flowerGFX;
    public TilemapEffectManager tilemapEffectManager;

    void Start()
    {        
        Collider2D[] results = Physics2D.OverlapPointAll(transform.position);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("Wall"))
                isObstacle = true; 
            if (item.gameObject.CompareTag("Decoration"))
                isObstacle = true; 
        }
        if (!isFlowering || isObstacle)
            flowerGFX.Uproot();
    }

    public void SetFlowerGFX(FlowerGFXData fgd)
    {
        flowerGFX.SetFlowerGFX(fgd);
    }

    public void TrySpreadingAround()
    {  
        CallEvaluation(transform.position + new Vector3(1, 0));
        CallEvaluation(transform.position + new Vector3(-1, 0));
        CallEvaluation(transform.position + new Vector3(0, 1));
        CallEvaluation(transform.position + new Vector3(0, -1));
    }

    void CallEvaluation(Vector3 position)
    {
        GameObject goRef = tilemapEffectManager.GetParticleCollider(position);
        if (goRef != null)
            goRef.GetComponent<TileFlowerManager>().EvaluateFloweringEligibility();
    }

    public void EvaluateFloweringEligibility()
    {
        if (isFlowering || isObstacle)
            return;
        if (!IsSourrandedByAtLeastOneBurntTile())
            StartFlowering();
    }

    public void StartFlowering()
    {
        StartCoroutine(DelayedFlowering());
    }

    IEnumerator DelayedFlowering()
    {
        yield return new WaitForSeconds(0.05f);
        isFlowering = true;
        flowerGFX.Plant();
    }

    public void StopFlowering()
    {
        isFlowering = false;
        flowerGFX.Uproot();
    }

    bool IsSourrandedByAtLeastOneBurntTile()
    {
        for (float y = transform.position.y - 1; y <= transform.position.y + 1; y++)
        {
            for (float x = transform.position.x - 1; x <= transform.position.x + 1; x++)
            {
                if (tilemapEffectManager.IsTileBurnt(new Vector3(x, y)))
                    return true;
            }
        }
        return false;
    }
}
