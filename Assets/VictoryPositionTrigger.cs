using UnityEngine;

public class VictoryPositionTrigger : MonoBehaviour
{

    Collider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    public void ActivateCollider()
    {
        boxCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementPath>().OnVictorySpot();
        }
    }
}
