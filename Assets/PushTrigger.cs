using System.Collections;
using UnityEngine;

public class PushTrigger : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    public CircleCollider2D triggerComponent;
    LinearMovement linearMovement;
    public bool isObstacle = false;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        linearMovement = GetComponent<LinearMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 playerPosition = other.transform.position;
            Vector3 destinationDirection = (transform.position - playerPosition).normalized;
            Vector3 destination = playgroundManager.GetCellCenter(transform.position + destinationDirection);
            if(isObstacle)
            {
                if (!playgroundManager.IsObstacleForRock(destination))
                {
                    linearMovement.MoveTo(destination, 0.3f);
                    triggerComponent.enabled = false;
                    StartCoroutine(ActivateTriggerDelay(0.3f));
                }
            } else {
                if (!playgroundManager.IsObstacle(destination))
                {
                    linearMovement.MoveTo(destination, 0.3f);
                    triggerComponent.enabled = false;
                    StartCoroutine(ActivateTriggerDelay(0.3f));
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
            AstarPath.active.Scan();
    }
}
