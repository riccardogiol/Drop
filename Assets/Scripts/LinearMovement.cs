using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 finalPosition;
    public float timer = 2;
    private float elapsedTime;
    private bool isMoving = false;

    public Vector3 startingScale;
    public Vector3 finalScale;

    void Awake()
    {
        startingScale = finalScale = transform.localScale;
    }

    public void MoveTo(Vector3 finalPos, float time)
    {
        startingPosition = transform.position;
        finalPosition = finalPos;
        timer = time;
        elapsedTime = 0;
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > timer)
            {
                isMoving = false;
                transform.position = finalPosition;
                transform.localScale = finalScale;
                this.enabled = false;
                return;
            }
            transform.position = Vector3.Lerp(startingPosition, finalPosition, elapsedTime/timer);
            transform.localScale = Vector3.Lerp(startingScale, finalScale, elapsedTime/timer);
        }

    }
}
