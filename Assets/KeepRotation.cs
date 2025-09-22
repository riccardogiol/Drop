using UnityEngine;

public class KeepRotation : MonoBehaviour
{

    private Quaternion initialRotation;
    public int eulerRotation = -999;

    void Start()
    {
        if (eulerRotation != -999)
            initialRotation = Quaternion.Euler(0, 0, eulerRotation);
        else
            initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
