using UnityEngine;

public class MortarBombManager : MonoBehaviour
{
    public GameObject targetSpot;

    public void SetTargetSpot(Transform target)
    {
        targetSpot.transform.position = target.position;
    }
}
