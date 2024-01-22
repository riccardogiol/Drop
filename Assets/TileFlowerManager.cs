using UnityEngine;

public class TileFlowerManager : MonoBehaviour
{
    public bool isFlowering = false;
    bool isBush = false;

    public SpriteRenderer spriteRenderer;

    public float checkSpreadingEligibilityInterval = 2f;
    float checkSpreadingEligibilityTimer;

    public RuleTileStateManager walkTilemap;

    void Start()
    {
        checkSpreadingEligibilityTimer = checkSpreadingEligibilityInterval;
        
        Collider2D[] results = Physics2D.OverlapPointAll(transform.position);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("Wall"))
                isBush = true; //disabling this should be way lighter
        }
        if (!isFlowering || isBush)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }

    void Update()
    {
        if (!isFlowering || isBush)
            return;
        checkSpreadingEligibilityTimer -= Time.deltaTime;
        if (checkSpreadingEligibilityTimer < 0)
        {
            CallEvaluation(transform.position + new Vector3(1, 0));
            CallEvaluation(transform.position + new Vector3(-1, 0));
            CallEvaluation(transform.position + new Vector3(0, 1));
            CallEvaluation(transform.position + new Vector3(0, -1));
            checkSpreadingEligibilityTimer = checkSpreadingEligibilityInterval;
        }
    }

    void CallEvaluation(Vector3 position)
    {
        GameObject goRef = walkTilemap.GetParticleCollider(position);
        if (goRef != null)
            goRef.GetComponent<TileFlowerManager>().EvaluateFloweringEligibility();
    }

    public void EvaluateFloweringEligibility()
    {
        if (isFlowering || isBush)
            return;
        if (!IsSourrandedByAtLeastOneBurntTile())
        {
            StartFlowering();
        }
    }

    public void StartFlowering()
    {
        isFlowering = true;
        checkSpreadingEligibilityTimer = checkSpreadingEligibilityInterval;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void StopFlowering()
    {
        isFlowering = false;
        checkSpreadingEligibilityTimer = checkSpreadingEligibilityInterval;
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    bool IsSourrandedByAtLeastOneBurntTile()
    {
        for (float y = transform.position.y - 1; y <= transform.position.y + 1; y++)
        {
            for (float x = transform.position.x - 1; x <= transform.position.x + 1; x++)
            {
                if (walkTilemap.IsTileBurnt(new Vector3(x, y)))
                    return true;
            }
        }
        return false;
    }
}
