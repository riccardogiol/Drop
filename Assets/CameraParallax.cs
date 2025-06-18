using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    GameObject mainCamera;
    public float xMult = 1.0f, yMult = 1.0f;
    public float xOffset, yOffset = 0.0f;
    float xOrigin, yOrigin;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        xOrigin = transform.position.x;
        yOrigin = transform.position.y;
        transform.position = new Vector3(xOrigin + xOffset + (mainCamera.transform.position.x - xOrigin) * xMult, yOrigin + yOffset + (mainCamera.transform.position.y - yOrigin) * yMult);
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(xOrigin + xOffset + (mainCamera.transform.position.x - xOrigin) * xMult, yOrigin + yOffset + (mainCamera.transform.position.y - yOrigin) * yMult);
    }
}
