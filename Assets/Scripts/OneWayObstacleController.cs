using UnityEngine;
using Pathfinding;

public class OneWayObstacleController : MonoBehaviour
{
    public bool blockingFromBelow;
    public bool blockingFromAbove;
    public bool blockingFromLeft;
    public bool blockingFromRight;

    SpriteRenderer spriteRenderer;
    PlatformEffector2D platformEffector2D;

    public Sprite spriteAbove;
    public Sprite spriteBelow;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformEffector2D = GetComponent<PlatformEffector2D>();

        if (blockingFromAbove)
        {
            spriteRenderer.sprite = spriteAbove;
            platformEffector2D.rotationalOffset = 0;
            if (Random.value > 0.5)
                spriteRenderer.flipX = true;
        }
        else if (blockingFromBelow)
        {
            spriteRenderer.sprite = spriteBelow;
            platformEffector2D.rotationalOffset = 180;
            if (Random.value > 0.5)
                spriteRenderer.flipX = true;
        }
        else if (blockingFromLeft)
        {
            spriteRenderer.sprite = spriteLeft;
            platformEffector2D.rotationalOffset = 90;
            if (Random.value > 0.5)
            {
                spriteRenderer.sprite = spriteRight;
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            spriteRenderer.sprite = spriteRight;
            platformEffector2D.rotationalOffset = 270;
            if (Random.value > 0.5)
            {
                spriteRenderer.sprite = spriteLeft;
                spriteRenderer.flipX = true;
            }
        }

        UpdateCollider();
    }

    public void UpdateCollider()
    {
        var gridGraph = AstarPath.active.data.gridGraph;

        if (blockingFromAbove)
        {
            GridNode targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y + 1);
            if (targetNode == null) return;
            targetNode.SetConnectionInternal(0, false);
        }
        else if (blockingFromBelow)
        {
            GridNode targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y - 1);
            if (targetNode == null) return;
            targetNode.SetConnectionInternal(2, false);
        }
        else if (blockingFromLeft)
        {
            GridNode targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x - 1, (int)transform.position.y);
            if (targetNode == null) return;
            targetNode.SetConnectionInternal(1, false);
        }
        else
        {
            GridNode targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x + 1, (int)transform.position.y);
            if (targetNode == null) return;
            targetNode.SetConnectionInternal(3, false);
        }
    }

    public bool IsBlockingFrom(Vector3 target)
    {

        if (blockingFromLeft)
            if (target.x < transform.position.x - 0.4 || target.y > transform.position.y + 0.4 || target.y < transform.position.y - 0.4)
                return true;
        if (blockingFromRight)
            if (target.x > transform.position.x + 0.4 || target.y > transform.position.y + 0.4 || target.y < transform.position.y - 0.4) 
                return true;
        if (blockingFromBelow)
            if (target.y < transform.position.y - 0.4 || target.x < transform.position.x - 0.4 || target.x > transform.position.x + 0.4)
                return true;
        if (blockingFromAbove)
            if (target.y > transform.position.y + 0.4 || target.x < transform.position.x - 0.4 || target.x > transform.position.x + 0.4)
                return true;
        return false;
    }

}
