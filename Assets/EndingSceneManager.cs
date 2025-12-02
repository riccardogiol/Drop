using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public MapMessageManager mmm;
    public MapMoveCamera mmc;
    GameObject target;

    public PlayerAnimationManager playerAnimator;

    public CinemachineDollyCart cart;

    public float totalSeconds = 20;
    float countup = 0;

    void Start()
    {
        if (PlayerPrefs.GetInt("TriggerEndingScene", 0) == 0)
            enabled = false;
    }
    void Update()
    {
        if (target == null)
        {
            mmm.StartEndingScene();
            mmc.SetInMoveCamera(true);
            target = cinemachine.Follow.gameObject;
            target.transform.parent = cart.transform;
            target.transform.localPosition = new Vector3(0, 0, 0);
            mmc.MoveCameraToPosition(target.transform.position);
            playerAnimator.transform.parent.transform.position = new Vector3(22, 30, 0);
            playerAnimator.PlayTriumph();
        }
        countup += Time.deltaTime;
        cart.m_Position = countup/totalSeconds;
        if (countup >= totalSeconds)
            StartCoroutine(EndingSceneDelayed());
    }

    IEnumerator EndingSceneDelayed()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("EndingScene");
    }
}
