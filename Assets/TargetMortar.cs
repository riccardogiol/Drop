using System;
using UnityEngine;

public class TargetMortar : MonoBehaviour
{
    public Transform target;
    //public float vyf = -2f;
    public float movementTime = 3.0f;
    public float height = 4.0f;
    //public bool rotateSprite = false;

    float ay1, ay2, yMax, viy, currentY, startingY, startingX, currentX, vx;
    float t2, currentT = 0.0f, currentT2;

    //Vector2 direction;

    void Start()
    {
        //direction = (target.position - transform.position).normalized;
        startingX = transform.position.x;
        startingY = transform.position.y;
        yMax = Math.Max(startingY, target.position.y);
        yMax += height;
        t2 = movementTime / 2f;
        float Dy = yMax - startingY;
        viy = 2 * Dy / t2;
        ay1 = -viy / t2;

        Dy = target.position.y - yMax;
        float vf2 = 2 * Dy / t2;
        ay2 = vf2 / t2;

        vx = (target.position.x - startingX) / movementTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentT += Time.fixedDeltaTime;

        currentX = startingX + vx * currentT;

        if (currentT <= t2)
            currentY = startingY + viy * currentT + 0.5f * ay1 * currentT * currentT;
        else if (currentT <= movementTime)
        {
            currentT2 = currentT - t2;
            currentY = yMax + 0.5f * ay2 * currentT2 * currentT2;
        }
        else
            Destroy(gameObject);

        transform.position = new Vector3(currentX, currentY);
    }
}
