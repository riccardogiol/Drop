using UnityEngine;
using Pathfinding;
using System;
using System.Collections.Generic;

public class EnemyAIPatrolMovement : MonoBehaviour
{
    public Vector3[] targets;
    public float jumpInterval = 2f;  
    public float jumpSpeed = 0.5f;
    float countdown = 0; // use jumpspeed as timer for take an action

    Vector3 currentTarget;
    int currentTargetIndex = 0;

    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    Vector3 destination = new Vector3();
    Vector3 nextDirection = new Vector3();
    bool retryUpdatePath = false;

    LinearMovement lm;
    SpriteFacing spriteFacing;

    public GameObject targetGFX;
    List<GameObject> targetsGFXGO = new List<GameObject>();

    void Start()
    {
        seeker = GetComponent<Seeker>();
        lm = GetComponent<LinearMovement>();
        spriteFacing = GetComponent<SpriteFacing>();

        spriteFacing.changeSide(new Vector3(0, -1, 0));

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            jumpInterval *= 1.3f;

        jumpSpeed = Math.Min(jumpSpeed, jumpInterval - 0.05f);
        currentTarget = targets[currentTargetIndex];

        UpdatePath(transform.position);
        countdown = jumpInterval;
    }

    void UpdatePath(Vector3 startingPosition)
    {
        path = null;
        if (seeker.IsDone())
            seeker.StartPath(startingPosition, currentTarget, OnPathComplete);
        else
            retryUpdatePath = true;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = jumpInterval;
            if (retryUpdatePath)
            {
                retryUpdatePath = false;
                UpdatePath(transform.position);
                return;
            }
            if (path == null)
                return;

            // when I evaluate the new path the waypoint in 0 is under me. Skip it
            if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < 0.1)
                currentWaypoint = Math.Min(currentWaypoint + 1, path.vectorPath.Count - 1);

            destination = path.vectorPath[currentWaypoint];

            nextDirection = (destination - transform.position).normalized;
            spriteFacing.changeSide(nextDirection);

            destination = path.vectorPath[currentWaypoint];
            lm.MoveTo(destination, jumpSpeed);

            if (currentWaypoint < path.vectorPath.Count - 1)
                currentWaypoint++;
            else
            {
                NextTarget();
                UpdatePath(destination);
            }
        }
        
    }

    void NextTarget()
    {
        currentTargetIndex++;
        if (currentTargetIndex >= targets.Length)
            currentTargetIndex = 0;
        currentTarget = targets[currentTargetIndex];
    }

    // visuals functions
    public void ShowPath()
    {
        int counter = 1;
        foreach (Vector3 target in targets)
        {
            GameObject goRef = Instantiate(targetGFX, target, Quaternion.identity);
            goRef.GetComponent<SetText>().SetInt(counter);
            goRef.transform.parent = transform.parent;
            targetsGFXGO.Add(goRef);
            counter++;
        }
    }

    public void HidePath()
    {
        foreach (GameObject go in targetsGFXGO)
            Destroy(go);
        targetsGFXGO.Clear();
    }
}
