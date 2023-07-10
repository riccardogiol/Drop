using UnityEngine;

public class EnemyGroundInteraction : MonoBehaviour
{
    public PlaygroundManager playground;
    
    public void NewPosition()
    {
        //take damage?
        playground.FireOnPosition(transform.position);
        //GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
