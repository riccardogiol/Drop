using UnityEngine;

public class EnemySwitchBehaviourOnPlayerHealth : MonoBehaviour
{
    public PlayerHealth playerHealth;
    EnemyHealth myHealth;

    EnemyAIChasingMovement chasing;
    EnemyAIPatrolMovement patrol;

    void Start()
    {
        myHealth = GetComponent<EnemyHealth>();
        chasing = GetComponent<EnemyAIChasingMovement>();
        patrol = GetComponent<EnemyAIPatrolMovement>();
    }

    void Update()
    {
        if (playerHealth.currentHealth < myHealth.currentHealth)
        {
            chasing.enabled = true;
            patrol.enabled = false;
        } else {
            chasing.enabled = false;
            patrol.enabled = true;
        }
        
    }
}
