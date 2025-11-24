using UnityEngine;

public class UnscaleLinearMovement : MonoBehaviour
{
    public Vector2 startingPosition;
    public Vector2 finalPosition;
    public float timer = 0.5f;

    private float elapsedTime;
    private bool isMoving = false;

    public void MoveTo(Vector3 finalPos, float time)
    {
        enabled = true;
        startingPosition = transform.position;
        finalPosition = finalPos;
        timer = time;
        elapsedTime = 0;
        isMoving = true;
    }

    public void ReverseMovement(float time)
    {
        MoveTo(startingPosition, time);
    }

    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime > timer)
            {
                isMoving = false;
                transform.position = finalPosition;
                enabled = false;
                return;
            }
            transform.position = Vector3.Lerp(startingPosition, finalPosition, elapsedTime/timer);
        }

    }
}
