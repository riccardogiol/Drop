using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class EagleEyeMode : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public Animator camAnimator;
    public Camera cam;
    public GameObject targetPrefab;
    
    GameObject target;
    Transform originalTarget;
    float originalOrtographicSize = 5f;
    public static bool inEagleMode;
    Vector3 camStartingPosition;
    Vector3 targetStartingPosition;
    PlaygroundManager playgroundManager;

    void Start()
    {
        originalTarget = cinemachine.Follow;
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
    }

    void Update()
    {
        if(!inEagleMode)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            camStartingPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            targetStartingPosition = target.transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = camStartingPosition - cam.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = targetStartingPosition + difference;
        }
    }

    public void Enter()
    {
        target = Instantiate(targetPrefab, originalTarget.position, quaternion.identity);
        cinemachine.LookAt = target.transform;
        cinemachine.Follow = target.transform;
        inEagleMode = true;
        playgroundManager.ShowEnergy();
        camAnimator.SetTrigger("EnterEagleEye");
    }

    
    public void Exit()
    {
        Destroy(target);
        cinemachine.m_Lens.OrthographicSize = originalOrtographicSize;
        cinemachine.LookAt = originalTarget;
        cinemachine.Follow = originalTarget;
        inEagleMode =false;
        playgroundManager.HideEnergy();
        camAnimator.SetTrigger("ExitEagleEye");
    }
}
