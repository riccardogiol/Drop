using UnityEngine;
using Pathfinding;

public class OneWayObstacleControllerNew : MonoBehaviour
{
    public DirectionOWO blockingFrom;

    public SpriteRenderer spriteRenderer;
    public PlatformEffector2D platformEffector2D;

    public Sprite spriteAbove;
    public Sprite spriteBelow;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    void Awake()
    {
        switch (blockingFrom)
        {
            case DirectionOWO.Above:
                spriteRenderer.sprite = spriteAbove;
                spriteRenderer.transform.localPosition = new Vector3(0, 0.2f, 0);
                platformEffector2D.rotationalOffset = 0;
                //platformEffector2D.transform.localPosition = new Vector3(0, 0.4f, 0);
                if (Random.value > 0.5)
                    spriteRenderer.flipX = true;
                break;
            case DirectionOWO.Below:
                spriteRenderer.sprite = spriteBelow;
                spriteRenderer.transform.localPosition = new Vector3(0, -0.2f, 0);
                platformEffector2D.rotationalOffset = 180;
                if (Random.value > 0.5)
                    spriteRenderer.flipX = true;
                break;
            case DirectionOWO.Left:
                spriteRenderer.sprite = spriteLeft;
                spriteRenderer.transform.localPosition = new Vector3(-0.2f, 0, 0);
                platformEffector2D.rotationalOffset = 90;
                if (Random.value > 0.5)
                {
                    spriteRenderer.sprite = spriteRight;
                    spriteRenderer.flipX = true;
                }
                break;
            case DirectionOWO.Right:
                spriteRenderer.sprite = spriteRight;
                spriteRenderer.transform.localPosition = new Vector3(0.2f, 0, 0);
                platformEffector2D.rotationalOffset = 270;
                if (Random.value > 0.5)
                {
                    spriteRenderer.sprite = spriteLeft;
                    spriteRenderer.flipX = true;
                }
                break;
        }

        UpdateCollider();
    }

    public void UpdateCollider()
    {
        var gridGraph = AstarPath.active.data.gridGraph;
        GridNode targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x - 1, (int)transform.position.y);

        switch (blockingFrom)
        {
            case DirectionOWO.Left:
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x - 1, (int)transform.position.y);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(1, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y + 1);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(0, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y - 1);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(2, false);
                break;
            case DirectionOWO.Right:
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x + 1, (int)transform.position.y);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(3, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y + 1);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(0, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y - 1);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(2, false);
                break;
            case DirectionOWO.Above:
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y + 1);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(0, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x - 1, (int)transform.position.y);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(1, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x + 1, (int)transform.position.y);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(3, false);
                break;
            case DirectionOWO.Below:
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x, (int)transform.position.y - 1);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(2, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x - 1, (int)transform.position.y);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(1, false);
                targetNode = (GridNode)gridGraph.GetNode((int)transform.position.x + 1, (int)transform.position.y);
                if (targetNode == null) return;
                targetNode.SetConnectionInternal(3, false);
                break;
        }
    }

    public bool IsBlockingFrom(Vector3 target)
    {
        if (target.x < transform.position.x - 0.4 && blockingFrom == DirectionOWO.Left)
            return true;
        if (target.x > transform.position.x + 0.4 && blockingFrom == DirectionOWO.Right)
            return true;
        if (target.y < transform.position.y - 0.4 && blockingFrom == DirectionOWO.Below)
            return true;
        if (target.y > transform.position.y + 0.4 && blockingFrom == DirectionOWO.Above)
            return true;
        return false;
    }

}

public enum DirectionOWO
{
    Left,
    Right,
    Above,
    Below,
}
