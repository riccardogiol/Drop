using UnityEngine;
using Pathfinding;
using System;
using System.Collections.Generic;

public class EnemyAIChasingMovement : MonoBehaviour
{
    public Transform target;
    public float jumpInterval = 2f;
    public float jumpSpeed = 0.5f;
    float countdown = 0;

    Path path;
    int currentWaypoint = 0;
    float nextWaypointDistance = 0.1f;
    Seeker seeker;
    Vector3 destination = new Vector3();
    Vector3 nextDirection = new Vector3();
    Vector3 targetCell = new Vector3();
    bool retryUpdatePath = false;

    LinearMovement lm;
    SpriteFacing spriteFacing;
    PlaygroundManager playgroundManager;

    public GameObject waypointGFX;
    List<GameObject> waypointsGFXGO = new List<GameObject>();

    void Awake()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        if (target == null)
            target = FindFirstObjectByType<PlayerHealth>().transform;
        seeker = GetComponent<Seeker>();
        lm = GetComponent<LinearMovement>();
        spriteFacing = GetComponent<SpriteFacing>();
    }

    void Start()
    {
        spriteFacing.changeSide(new Vector3(0, -1, 0));

        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            jumpInterval *= 1.3f;

        UpdatePath(transform.position);
        countdown = jumpInterval;
    }

    void UpdatePath(Vector3 startingPosition)
    {
        path = null;
        if (seeker.IsDone())
        {
            targetCell = playgroundManager.GetCellCenter(target.position);
            seeker.StartPath(startingPosition, targetCell, OnPathComplete);
        }
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

            // se sono sopra il player stai li
            if (currentWaypoint >= path.vectorPath.Count)
                return;
            
            // se sono sopra la mia prossima destinazione, avanza di uno sulla lista 
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
                currentWaypoint = Math.Min(currentWaypoint + 1, path.vectorPath.Count - 1);

            destination = path.vectorPath[currentWaypoint];

            nextDirection = (destination - transform.position).normalized;
            if (nextDirection.magnitude > 0)
                spriteFacing.changeSide(nextDirection);

            lm.MoveTo(destination, jumpSpeed);
            if (currentWaypoint < path.vectorPath.Count - 1)
                currentWaypoint++;
            
            UpdatePath(destination);
        }
    }

    // visuals function
    public void ShowPath()
    {
        if (path == null)
            return;
        Vector3 direction = new Vector3(1, 0, 0);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        for (int i = currentWaypoint; i < path.vectorPath.Count; i++)
        {
            if (i < path.vectorPath.Count - 1)
            {
                Vector3 nextWaypoint = path.vectorPath[i + 1];
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
