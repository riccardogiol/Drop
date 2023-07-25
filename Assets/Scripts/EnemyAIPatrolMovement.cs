using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

public class EnemyAIPatrolMovement : MonoBehaviour
{
    public Vector3[] targets;
    public float jumpInterval = 2f;  
    public float jumpSpeed = 0.5f;

    Vector3 currentTarget;
    int currentTargetIndex = 0;

    Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 0.1f;

    Seeker seeker;
    LinearMovement lm;
    SpriteFacing spriteFacing;

    bool wasDisabled = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        lm = GetComponent<LinearMovement>();
        spriteFacing = GetComponent<SpriteFacing>();

        spriteFacing.changeSide(new Vector3(0, -1, 0));

        currentTarget = targets[currentTargetIndex];
        InvokeRepeating("UpdatePath", 0f, jumpInterval);
        
        StartCoroutine(NextStep());
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && this.enabled)
            seeker.StartPath(transform.position, currentTarget, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator NextStep()
    {
        while(true)
        {
            if (!this.enabled)
            {
                wasDisabled = true;
                yield return new WaitForSeconds(jumpInterval);
            }
            else
            {
                if (wasDisabled)
                {
                    yield return new WaitForSeconds(2f);
                    wasDisabled = false;
                }
                if (path == null)
                    yield return new WaitForSeconds(0.2f);
                if (currentWaypoint >= path.vectorPath.Count)
                    yield return new WaitForSeconds(jumpInterval);

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint = Math.Min(currentWaypoint + 1, path.vectorPath.Count - 1);
                
                Vector3 nextDirection = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                if(nextDirection.magnitude != 0)
                    spriteFacing.changeSide(nextDirection);
                lm.MoveTo(path.vectorPath[currentWaypoint], jumpSpeed);

                if (Vector2.Distance(transform.position, currentTarget) < nextWaypointDistance)
                    NextTarget();
                
                yield return new WaitForSeconds(jumpInterval);
            }
        }
    }

    void NextTarget()
    {
        currentTargetIndex++;
        if (currentTargetIndex >= targets.Length)
        {
            currentTargetIndex = 0;
        }
        currentTarget = targets[currentTargetIndex];
    }
}
