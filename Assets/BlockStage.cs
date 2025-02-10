using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockStage : MonoBehaviour
{
    public int stageBlocked;
    public Button stageButton;

    public string solvingCode;

    PlayerMovementKeysMap playerMovementKeysMap;

    void Start()
    {
        if (PlayerPrefs.GetInt(solvingCode, 0) == 0)
            StartCoroutine(OneFrameDelayBlockStage());
        else 
           enabled = false;
        
    }

    IEnumerator OneFrameDelayBlockStage()
    {
        yield return null;
        stageButton.interactable = false;
        playerMovementKeysMap = FindFirstObjectByType<PlayerMovementKeysMap>();
        playerMovementKeysMap.SetLastAvailableStage(stageBlocked - 1);
    }


    void Update()
    {
        if (PlayerPrefs.GetInt(solvingCode, 0) == 1)
        {
            stageButton.interactable = true;
            playerMovementKeysMap.SetLastAvailableStage(stageBlocked);
            enabled = false;
        }
        
    }
}
