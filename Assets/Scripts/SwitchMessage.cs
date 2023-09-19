using UnityEngine;

public class SwitchMessage : MonoBehaviour
{
    public GameObject nextMessage;
    public bool turnShaderOff = false;
    public GameObject shader;

    public void Switch()
    {
        if (turnShaderOff)
            shader.SetActive(false);

        nextMessage.SetActive(true);
        gameObject.SetActive(false);
        return;
    }
}
