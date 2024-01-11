using UnityEngine;
using UnityEngine.UI;

public class SetButtonActionWave : MonoBehaviour
{
    public Button button;
    public bool actionDisabled = false;
    public GameObject disableImage;
    PlayerWave playerWave;

    readonly string unlockingCode1 = "Lvl3";

    void Awake()
    {
        if(PlayerPrefs.GetInt(unlockingCode1, 0) == 0)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    
    void Start()
    {
        playerWave = FindFirstObjectByType<PlayerWave>();
        if (playerWave == null)
        {
            Debug.LogWarning("Player WAVE button will not work since No object of type Player wave found");
            return;
        }
        button.onClick.AddListener(delegate {playerWave.TryWaveAttack();});
        if (actionDisabled)
        {
            button.interactable = false;
            disableImage.SetActive(true);
        }
    }
}
