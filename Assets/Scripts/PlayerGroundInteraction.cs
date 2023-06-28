using UnityEngine;

public class PlayerGroundInteraction : MonoBehaviour
{
    public PlaygroundManager playground;
    
    public void NewPosition()
    {
        int damage = playground.WaterOnPosition(transform.position);
        GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
