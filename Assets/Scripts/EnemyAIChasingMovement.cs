using UnityEngine;
using Pathfinding;
using System.Collections;
using System;
using System.Collections.Generic;

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
    SpriteFacing spriteFacing;

    bool wasDisabled = false;

    public GameObject waypointGFX;
    List<GameObject> waypointsGFXGO = new List<GameObject>();

    void Start()
    {
        if (target == null)
            target = FindFirstObjectByType<PlayerHealth>().transform;
        seeker = GetComponent<Seeker>();
        lm = GetComponent<LinearMovement>();
        spriteFacing = GetComponent<SpriteFacing>();

        spriteFacing.changeSide(new Vector3(0, -1, 0));
        
        InvokeRepeating("UpdatePath", 0f, jumpInterval);
        
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
                Vector3 nextDirection = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                if (nextDirection.magnitude != 0)
                    spriteFacing.changeSide(nextDirection);

                lm.MoveTo(path.vectorPath[currentWaypoint], jumpSpeed);
                
                yield return new WaitForSeconds(jumpInterval);
            }
        }
    }

    public void ShowPath()
    {
        if (path == null)
            return;
        Vector3 direction = new Vector3(1, 0, 0);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        for(int i = currentWaypoint; i < path.vectorPath.Count; i++)
        {
            if (i < path.vectorPath.Count - 1)
            {
                Vector3 nextWaypoint = path.vectorPath[i+1];
                direction = (nextWaypoint - path.vectorPath[i]).normalized;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }
            GameObject goRef = Instantiate(waypointGFX, path.vectorPath[i], Quaternion.identity);
            foreach (Transform child in goRef.transform)
                child.Rotate(0, 0, angle);
            goRef.transform.parent = transform.parent;
            waypointsGFXGO.Add(goRef);
        }
    }

    public void HidePath()
    {
        foreach (GameObject go in waypointsGFXGO)
            Destroy(go);
        waypointsGFXGO.Clear();
    }
}
