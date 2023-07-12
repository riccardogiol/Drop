using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

public class EnemyAIChasingMovement : MonoBehaviour
{
    public Transform target;
    public float jumpInterval = 2f;  
    public float jumpSpeed = 0.5f;

    Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 0.1f;

    Seeker seeker;
    LinearMovement lm;

    bool wasDisabled = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        lm = GetComponent<LinearMovement>();
        
        InvokeRepeating("UpdatePath", 0f, 2f);
        
        StartCoroutine(NextStep());
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && this.enabled)
            seeker.StartPath(transform.position, target.position, OnPathComplete);
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
                lm.MoveTo(path.vectorPath[currentWaypoint], jumpSpeed);
                
                yield return new WaitForSeconds(jumpInterval);
            }
        }
    }
}
