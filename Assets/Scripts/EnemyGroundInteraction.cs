using System.Collections;
using UnityEngine;

public class EnemyGroundInteraction : MonoBehaviour
{
    public PlaygroundManager playground;
    public float checkingInterval = 0.5f;

    Vector3 oldPosition;

    void Awake()
    {
        oldPosition = transform.position;
    }

    void Start()
    {
        StartCoroutine(NewPosition());
    }
    
    IEnumerator NewPosition()
    {
        while(true)
        {
            //take damage?
            playground.FireOnPosition(transform.position);
            //GetComponent<PlayerHealth>().TakeDamage(damage);

            if ((transform.position - oldPosition).magnitude > 0.9f)
            {
                if (Random.value > 0.5)
                {
                    playground.FlameOnPosition(oldPosition);
                }
                oldPosition = transform.position;
            }

            yield return new WaitForSeconds(checkingInterval);
        }
    }
}
