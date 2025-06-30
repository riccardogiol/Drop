using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    PlaygroundManager playground;
    PlayerShield playerShield;
    PlayerMovementInterruption playerMovementInterruption;

    Vector3 lastCellTouched = new Vector3(0, 0, 0);
    Vector3 secondLastCellTouched = new Vector3(0, 0, 0);
    Vector3 grassPosition = new Vector3(0, 0, 0);

    void Start()
    {
        playground = FindObjectOfType<PlaygroundManager>();
        playerShield = GetComponent<PlayerShield>();
        playerMovementInterruption = GetComponent<PlayerMovementInterruption>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Flame":
                int flameEnergy = other.GetComponent<PickFlame>().energy;
                gameObject.GetComponent<PlayerHealth>().TakeDamage(flameEnergy);
                other.GetComponent<PickFlame>().DestroyFlame();
                break;
            case "Enemy":
                if (playerShield != null)
                {
                    if (playerShield.isActive)
                    {
                        playerMovementInterruption.Rebounce(secondLastCellTouched);
                        // play shield grafics
                        return;
                    }
                }
                int enemyHealth = other.GetComponent<EnemyHealth>().currentHealth;
                int playerHealth = gameObject.GetComponent<PlayerHealth>().currentHealth;
                other.GetComponent<EnemyHealth>().TakeDamage(playerHealth);
                gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyHealth);
                break;
            case "Grass":
                playground.WaterOnPosition(other.transform.position);
                grassPosition = other.transform.position;
                if (lastCellTouched != grassPosition)
                {
                    secondLastCellTouched = lastCellTouched;
                    lastCellTouched = grassPosition;
                }
                break;
        }
    }
}
