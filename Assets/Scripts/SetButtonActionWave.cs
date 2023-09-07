using UnityEngine;
using UnityEngine.UI;

public class SetButtonActionWave : MonoBehaviour
{
    public Button button;
    PlayerWave playerWave;

    void Start()
    {
        playerWave = FindFirstObjectByType<PlayerWave>();
        if (playerWave == null)
        {
            Debug.LogWarning("Player WAVE button will not work since No object of type Player wave found");
            return;
        }
        button.onClick.AddListener(delegate {playerWave.TryWaveAttack();});
    }
}
