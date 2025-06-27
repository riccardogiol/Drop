using System.Collections;
using UnityEngine;
using Pathfinding;

public class PushTrigger : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    public CircleCollider2D triggerComponent;
    LinearMovement linearMovement;

    public bool isObstacle = false;
    Vector3 origin;
    OneWayObstacleController[] oneWayComps;
    float speed = 0.15f;
    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        linearMovement = GetComponent<LinearMovement>();
        if (isObstacle)
            oneWayComps = FindObjectsOfType<OneWayObstacleController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 playerPosition = other.transform.position;
            origin = transform.position;
            Vector3 destinationDirection = (origin - playerPosition).normalized;
            Vector3 destination = playgroundManager.GetCellCenter(transform.position + destinationDirection);
            if(isObstacle)
            {
                if (!playgroundManager.IsObstacleForRock(destination))
                {
                    linearMovement.MoveTo(destination, speed);
                    triggerComponent.enabled = false;
                    StartCoroutine(ActivateTriggerDelay(speed));
                }
                else
                {
                    // player go back to player position
                }
            } else {
                if (!playgroundManager.IsObstacle(destination))
                {
                    linearMovement.MoveTo(destination, speed);
                    triggerComponent.enabled = false;
                    StartCoroutine(ActivateTriggerDelay(speed));
                }
                else
                {
                    other.GetComponent<PlayerMovementKeys>().InterruptMovement(0.2f);
                }
            }
        }
        if(other.CompareTag("Enemy"))
        {
            if (isObstacle)
                other.GetComponent<LinearMovement>().ReverseMovement(0.2f);
        }
    }

    IEnumerator ActivateTriggerDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        triggerComponent.enabled = true;
        if (isObstacle)
        {
            Collider2D obstacleCollider = GetComponent<Collider2D>();
            Bounds bounds = obstacleCollider.bounds;
            bounds.Expand(2.0f);
            GraphUpdateObject guo = new GraphUpdateObject(bounds)
            {
                updatePhysics = true
            };
            AstarPath.active.UpdateGraphs(guo);
            AstarPath.active.FlushGraphUpdates();
            
            foreach(OneWayObstacleController oneWayComp in oneWayComps)
                oneWayComp.UpdateCollider();
        }
    }
}
