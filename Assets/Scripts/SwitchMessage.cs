using UnityEngine;

public class SwitchMessage : MonoBehaviour
{
    public GameObject previousMessage, nextMessage;
    public bool turnShaderOff = false;
    public GameObject shader;

    public void Switch()
    {
        if (turnShaderOff)
            shader.SetActive(false);

        nextMessage.SetActive(true);
        if (previousMessage == null)
            gameObject.SetActive(false);
        else
            previousMessage.SetActive(false);
        return;
    }
}
