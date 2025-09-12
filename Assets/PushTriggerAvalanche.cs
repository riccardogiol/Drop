using System.Collections;
using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class PushTriggerAvalanche : MonoBehaviour
{
    PlaygroundManager playgroundManager;
    LinearMovement linearMovement;

    OneWayObstacleController[] oneWayComps;
    OneWayObstacleControllerNew[] oneWayCompsNew;
    float movementTime = 0.2f;

    public Vector3 destinationDirection;
    public GameObject directionArrow;
    float scale = 0.2f;

    bool isMoving = false;
    public List<Animator> rockAnims;

    CameraAnimationManager cameraAnimationManager;

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        linearMovement = GetComponent<LinearMovement>();
        oneWayComps = FindObjectsOfType<OneWayObstacleController>();
        oneWayCompsNew = FindObjectsOfType<OneWayObstacleControllerNew>();
        cameraAnimationManager = FindFirstObjectByType<CameraAnimationManager>();

        foreach (Animator anim in rockAnims)
            anim.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        if (destinationDirection.x == 1)
            {
                directionArrow.GetComponent<SinLoopMovement>().scaleX = scale;
            }
            else if (destinationDirection.x == -1)
            {
                directionArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
                directionArrow.GetComponent<SinLoopMovement>().scaleX = scale;
            }
            else if (destinationDirection.y == 1)
            {
                directionArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
                directionArrow.GetComponent<SinLoopMovement>().scaleY = scale;
            }
            else
            {
                directionArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 270);
                directionArrow.GetComponent<SinLoopMovement>().scaleY = scale;
            }
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
                other.GetComponent<PickWaterdrop>().DestroyWaterdrop();
                break;
            case "Superdrop":
                other.GetComponent<PickSuperdrop>().DestroySuperdrop();
                break;
            case "Flame":
                other.GetComponent<PickFlame>().DestroyFlame();
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
                foreach(var rock in rockAnims)
                    rock.SetTrigger("StartRolling");
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
            obstacleCollider.isTrigger = false;
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
            foreach (OneWayObstacleControllerNew oneWayCompNew in oneWayCompsNew)
                oneWayCompNew.UpdateCollider();
            
            
            foreach(var rock in rockAnims)
                rock.SetTrigger("StopRolling");
            if (cameraAnimationManager != null)
                cameraAnimationManager.StartTilting();
            directionArrow.SetActive(false);
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
