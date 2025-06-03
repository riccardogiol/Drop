using UnityEngine;

public class SwitchObjOnStart : MonoBehaviour
{
    public GameObject objSwitchOff;
    public GameObject objSwitchOn;

    void Start()
    {
        if (objSwitchOff != null)
            objSwitchOff.SetActive(false);
        if (objSwitchOn != null)
            objSwitchOn.SetActive(true);
        
    }
}
