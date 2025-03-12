using UnityEngine;

public class TriggerFlowering : MonoBehaviour
{
    GameObject particleCollideGO;

    void Start()
    {
        Collider2D[] results = Physics2D.OverlapPointAll(transform.position);
        foreach(Collider2D item in results)
            if (item.gameObject.CompareTag("ParticleCollider"))
                particleCollideGO = item.gameObject;

        if (particleCollideGO != null)
           InvokeRepeating("StartFlowering", 0, 2.0f);
    }


    void StartFlowering()
    {
        particleCollideGO.GetComponent<TileFlowerManager>().StartFlowering();
    }
}
