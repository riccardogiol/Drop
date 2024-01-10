using UnityEngine;

public class CameraAnimationManager : MonoBehaviour
{
    public Animator cameraAnimator;
    PlayerMapTargeting playerMapTargeting;

    void Start()
    {
        playerMapTargeting = GetComponent<PlayerMapTargeting>();
    }
    public void StartEndingAnimation()
    {
        cameraAnimator.SetTrigger("ZoomIn");
        playerMapTargeting.FromTargetToPlayer();
    }
}
