using UnityEngine;
using UnityEngine.UI;

public class SetButtonActionShoot : MonoBehaviour
{
    public Button button;
    public bool actionDisabled = false;
    public GameObject disableImage;
    PlayerShooting playerShooting;

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
        playerShooting = FindFirstObjectByType<PlayerShooting>();
        if (playerShooting == null)
        {
            Debug.LogWarning("Player shooting button will not work since No object of type Player shooting found");
            return;
        }
        button.onClick.AddListener(delegate {playerShooting.TryShoot();});
        if (actionDisabled)
        {
            button.interactable = false;
            disableImage.SetActive(true);
        }
    }
}
