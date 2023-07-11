using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyAIMovement : MonoBehaviour
{
    public Transform target;
    public float jumpInterval = 2f;
    public float nextWaypointDistance = 0.1f;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;
    LinearMovement lm;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        lm = GetComponent<LinearMovement>();
        
        InvokeRepeating("UpdatePath", 0f, 2f);
        
        StartCoroutine(NextStep());
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

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
            if (path == null)
            {
                yield return new WaitForSeconds(0.2f);
            }
            if (currentWaypoint >= path.vectorPath.Count)
            {
                yield return new WaitForSeconds(jumpInterval);
            }
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            lm.enabled = true;
            lm.MoveTo(path.vectorPath[currentWaypoint], 0.5f);
            
            yield return new WaitForSeconds(jumpInterval);
        }
    }
    /*
    void FixedUpdate()
    {
        

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
    }
    */
}
