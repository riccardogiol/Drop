using UnityEngine;

public class RandomizeRotation : MonoBehaviour
{
    public Transform GOToRotate;
    public void Rotate()
    {
        GOToRotate.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}
