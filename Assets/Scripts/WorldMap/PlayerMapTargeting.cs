using System;
using System.Collections;
using UnityEngine;

public class PlayerMapTargeting : MonoBehaviour
{
    public GameObject target;
    PlaygroundManager playgroundManager;
    Transform playerPosition;


    float borderX, borderY;
    float maxX, maxY, minX, minY;
    bool isMoving = true;

    void Start()
    {
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        playerPosition = FindFirstObjectByType<PlayerHealth>().gameObject.transform;
        int playgroundMaxX = playgroundManager.maxX;
        int playgroundMaxY = playgroundManager.maxY;
        borderX = Math.Min(playgroundMaxX/2, 3f);
        borderY = Math.Min(playgroundMaxY/2, 3f);
        maxX = playgroundMaxX - borderX;
        maxY = playgroundMaxY - borderY;
        minX = borderX;
        minY = borderY;
        target.transform.position = playerPosition.position;

        StartCoroutine(FromPlayerToTarget());

    }

    IEnumerator FromPlayerToTarget()
    {
        yield return new WaitForSeconds(1);
        target.GetComponent<LinearMovement>().MoveTo(BoundedPosition(playerPosition.position), 1);
        yield return new WaitForSeconds(1);
        isMoving = false;

    }

    public void FromTargetToPlayer()
    {
        isMoving = true;
        target.GetComponent<LinearMovement>().MoveTo(playerPosition.position, 1);
    }

    void FixedUpdate()
    {
        if (!isMoving)
            target.transform.position = BoundedPosition(playerPosition.position);
    }

    Vector3 BoundedPosition(Vector3 position)
    {
        Vector3 newPos = new(0, 0, 0);
        newPos.x = Math.Max(Math.Min(position.x, maxX), minX);
        newPos.y = Math.Max(Math.Min(position.y, maxY), minY);
        return newPos;
    }
}
