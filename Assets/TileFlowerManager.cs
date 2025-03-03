using System.Collections;
using UnityEngine;

public class TileFlowerManager : MonoBehaviour
{
    public bool isFlowering = false;
    public bool isObstacle = false;
    public bool isSourrandedByFlowers;

    public FlowerGFXManager flowerGFX;
    public TilemapEffectManager tilemapEffectManager;

    public GameObject insectFlying;
    float timer = 5.0f;
    bool hasInsects = false;

    void Start()
    {        
        Collider2D[] results = Physics2D.OverlapPointAll(transform.position);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("Wall"))
                isObstacle = true; 
            if (item.gameObject.CompareTag("Decoration"))
                isObstacle = true; 
            if (item.gameObject.CompareTag("DecorationNoFire"))
                isObstacle = true; 
        }
        if (!isFlowering || isObstacle)
            flowerGFX.Uproot();
        isSourrandedByFlowers = false;
    }

    public void SetFlowerGFX(FlowerGFXData fgd)
    {
        flowerGFX.SetFlowerGFX(fgd);
    }

    public bool TrySpreadingAround()
    {
        if (isSourrandedByFlowers)
            return false;
        isSourrandedByFlowers = CallEvaluation(transform.position + new Vector3(1, 0));
        isSourrandedByFlowers = isSourrandedByFlowers && CallEvaluation(transform.position + new Vector3(-1, 0));
        isSourrandedByFlowers = isSourrandedByFlowers && CallEvaluation(transform.position + new Vector3(0, 1));
        isSourrandedByFlowers = isSourrandedByFlowers && CallEvaluation(transform.position + new Vector3(0, -1));
        return true;
    }

    bool CallEvaluation(Vector3 position)
    {
        GameObject goRef = tilemapEffectManager.GetParticleCollider(position);
        if (goRef != null)
            return goRef.GetComponent<TileFlowerManager>().EvaluateFloweringEligibility();
        return true;
    }

    public bool EvaluateFloweringEligibility()
    {
        if (isFlowering || isObstacle)
            return true;
        if (!IsSourrandedByAtLeastOneBurntTile())
        {
            StartFlowering();
            return true;
        }
        return false;
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
        if (Random.value < 0.2)
        {
            InvokeRepeating("SpawnInsectGFX", Random.Range(0.0f, 2.0f), timer);
            hasInsects = true;
        }
    }

    public void StopFlowering()
    {
        isFlowering = false;
        flowerGFX.Uproot();
        if (hasInsects)
        {
            CancelInvoke("SpawnInsectGFX");
            hasInsects = false;
        }
        GameObject auxGO;
        for (float y = transform.position.y - 1; y <= transform.position.y + 1; y++)
        {
            auxGO = tilemapEffectManager.GetParticleCollider(new Vector3(transform.position.x, y));
            if (auxGO != null)
                auxGO.GetComponent<TileFlowerManager>().isSourrandedByFlowers = false;
        }
        for (float x = transform.position.x - 1; x <= transform.position.x + 1; x++)
            {
                auxGO = tilemapEffectManager.GetParticleCollider(new Vector3(x, transform.position.y));
                if (auxGO != null)
                    auxGO.GetComponent<TileFlowerManager>().isSourrandedByFlowers = false;
            }
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

    void SpawnInsectGFX()
    {
        if (Random.value < 0.3)
        {
            GameObject goref = Instantiate(insectFlying, transform.position, Quaternion.identity);
            goref.transform.parent = transform;
        }
    }
}
