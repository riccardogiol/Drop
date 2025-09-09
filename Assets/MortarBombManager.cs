using UnityEngine;

public class MortarBombManager : MonoBehaviour
{
    public GameObject targetSpot;

    public void SetTargetSpot(Vector3 target)
    {
        targetSpot.transform.position = target;
    }
}
