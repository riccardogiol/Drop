using UnityEngine;

public class TriggerFlowering : MonoBehaviour
{
    void Update()
    {
        Collider2D[] results = Physics2D.OverlapPointAll(transform.position);
        foreach(Collider2D item in results)
        {
            if (item.gameObject.CompareTag("ParticleCollider"))
            {
                item.gameObject.GetComponent<TileFlowerManager>().StartFlowering();
                Destroy(gameObject);
            }
        }
    }
}
