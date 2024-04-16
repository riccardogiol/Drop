using UnityEngine.UI;
using UnityEngine;

public class SetButtonActionSuper : MonoBehaviour
{
    public Button button;
    public bool actionDisabled = false;
    public GameObject disableImage;
    PlayerSuperPower playerSuperPower;

    readonly string unlockingCode1 = "SuperPurchased";

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
        playerSuperPower = FindFirstObjectByType<PlayerSuperPower>();
        if (playerSuperPower == null)
        {
            Debug.LogWarning("Player super button will not work since No object of type Player super found");
            return;
        }
        button.onClick.AddListener(delegate {playerSuperPower.TryActivate();});
        if (actionDisabled)
        {
            button.interactable = false;
            disableImage.SetActive(true);
            playerSuperPower.enabled = false;
        }
    }
}
