using UnityEngine;

public class PlayerEventParamsManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerShooting playerShooting;
    public PlayerWave playerWave;

    public float GetPositionX()
    {
        return transform.position.x;
    }

    public float GetPositionY()
    {
        return transform.position.y;
    }

    public int GetHealth()
    {
        return playerHealth.currentHealth;
    }

    public int GetWaterBulletUsage()
    {
        if (playerShooting.enabled == false)
            return 0;
        return playerShooting.powerUsage;
    }

    public int GetWaveUsage()
    {
        if (playerWave.enabled == false)
            return 0;
        return playerWave.powerUsage;
    }
}
