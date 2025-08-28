using System.Collections;
using UnityEngine;
using Pathfinding;

public class PushTriggerAvalanche : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    LinearMovement linearMovement;

    OneWayObstacleController[] oneWayComps;
    float movementTime = 0.3f;

    public Vector3 destinationDirection;

    bool isMoving = false;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        linearMovement = GetComponent<LinearMovement>();
        oneWayComps = FindObjectsOfType<OneWayObstacleController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                TryToMoveInDirection();


                break;
            case "Enemy":
                if (isMoving)
                {
                    other.GetComponent<EnemyHealth>().TakeDamage(100);
                }
                else
                {
                    TryToMoveInDirection();
                }
                break;
            case "Wall":
                if (isMoving)
                {
                    playgroundManager.RemoveWall(other.transform.position);
                }
                break;
            case "Waterdrop":
            case "Superdrop":
            case "Firedrop":
                
                break;

        }
    }

    bool TryToMoveInDirection()
    {
        Vector3 destination = playgroundManager.GetCellCenter(transform.position + destinationDirection);

        if (!playgroundManager.IsObstacleForAvalanche(destination))
        {
            if (!isMoving)
            {
                isMoving = true;
                // change graphics
            }
            linearMovement.MoveTo(destination, movementTime);
            StartCoroutine(ActivateTriggerDelay(movementTime));
            return true;
        }
        else
        {
            tag = "DecorationNoFire";
            gameObject.layer = 6;
            isMoving = false;

            Collider2D obstacleCollider = GetComponent<Collider2D>();
            Bounds bounds = obstacleCollider.bounds;
            bounds.Expand(2.0f);
            GraphUpdateObject guo = new GraphUpdateObject(bounds)
            {
                updatePhysics = true
            };
            AstarPath.active.UpdateGraphs(guo);
            AstarPath.active.FlushGraphUpdates();

            foreach (OneWayObstacleController oneWayComp in oneWayComps)
                oneWayComp.UpdateCollider();
            // change grafics
            // enabled = false;
            return false;
        }
    }


    IEnumerator ActivateTriggerDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D obstacleCollider = GetComponent<Collider2D>();
        Bounds bounds = obstacleCollider.bounds;
        bounds.Expand(2.0f);
        GraphUpdateObject guo = new GraphUpdateObject(bounds)
        {
            updatePhysics = true
        };
        AstarPath.active.UpdateGraphs(guo);
        AstarPath.active.FlushGraphUpdates();

        foreach (OneWayObstacleController oneWayComp in oneWayComps)
            oneWayComp.UpdateCollider();

        TryToMoveInDirection();
    }
}
