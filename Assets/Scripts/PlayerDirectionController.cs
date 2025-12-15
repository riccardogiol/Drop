using System.Collections;
using UnityEngine;

public class PlayerDirectionController: MonoBehaviour
{
    public Animator playerAnimator;
    public Vector2 lastDirection = new(0, -1);

    float speed = 0;
    bool hitAnimation = false;

    LinearMovement playerGFXLM;

    void Awake()
    {
        playerGFXLM = playerAnimator.GetComponent<LinearMovement>();
        hitAnimation = false;
    }

    void Start()
    {
        playerAnimator.SetFloat("LastHorizontal", lastDirection.x);
        playerAnimator.SetFloat("LastVertical", lastDirection.y);
        InvokeRepeating("SpeedCheck", 0f, 0.05f);
    }

    public void UpdateDirection(Vector2 lastMovement)
    {
        if (lastMovement.magnitude > 0.3)
            {
                Vector2 newDirection = lastDirection;
                if (lastMovement.x > 0.5) {
                    newDirection = new Vector2(1, 0);
                } else if (lastMovement.x < -0.5){
                    newDirection = new Vector2(-1, 0);
                } else if (lastMovement.y > 0.5){
                    newDirection = new Vector2(0, 1);
                } else if (lastMovement.y < -0.5){
                    newDirection = new Vector2(0, -1);
                }
                if (lastDirection != newDirection)
                {
                    playerAnimator.SetFloat("LastHorizontal", newDirection.x);
                    playerAnimator.SetFloat("LastVertical", newDirection.y);
                }
                lastDirection = newDirection;
            }
            playerAnimator.SetFloat("Horizontal", lastMovement.x);
            playerAnimator.SetFloat("Vertical", lastMovement.y);
            speed = lastMovement.sqrMagnitude;
    }

    //TODO: not super clean solution
    void SpeedCheck()
    {
        playerAnimator.SetFloat("Speed", speed);
        speed = 0;
    }

    public void TurnClockwise()
    {
        if (lastDirection == new Vector2(0, 1))
            lastDirection = new Vector2(1, 0);
        else if (lastDirection == new Vector2(1, 0))
            lastDirection = new Vector2(0, -1);
        else if (lastDirection == new Vector2(0, -1))
            lastDirection = new Vector2(-1, 0);
        else if (lastDirection == new Vector2(-1, 0))
            lastDirection = new Vector2(0, 1);
        
        playerAnimator.SetFloat("LastHorizontal", lastDirection.x);
        playerAnimator.SetFloat("LastVertical", lastDirection.y);
        playerAnimator.SetFloat("Horizontal", lastDirection.x);
        playerAnimator.SetFloat("Vertical", lastDirection.y);
    }

    public void HitAnimation()
    {
        hitAnimation = true;
        float timer = 0.07f;
        Vector2 StartingPos = playerGFXLM.transform.localPosition;
        playerGFXLM.MoveTo(StartingPos + lastDirection * 0.2f, timer);
        StartCoroutine(ComesBack(StartingPos, timer));
    }

    IEnumerator ComesBack(Vector3 finalPosition, float timer)
    {
        yield return 0;
        yield return new WaitForSeconds(timer);
        playerGFXLM.MoveTo(finalPosition, timer);
        hitAnimation = false;
    }
}
