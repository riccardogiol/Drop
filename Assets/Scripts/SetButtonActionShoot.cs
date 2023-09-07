using UnityEngine;
using UnityEngine.UI;

public class SetButtonActionShoot : MonoBehaviour
{
    public Button button;
    PlayerShooting playerShooting;

    void Start()
    {
        playerShooting = FindFirstObjectByType<PlayerShooting>();
        if (playerShooting == null)
        {
            Debug.LogWarning("Player shooting button will not work since No object of type Player shooting found");
            return;
        }
        button.onClick.AddListener(delegate {playerShooting.TryShoot();});
    }
}
