using UnityEngine;
using UnityEngine.UI;

public class OnExitButtonSelection : MonoBehaviour
{

    public Button buttonToSelectOnExit;
    
    public void CloseMenu()
    {
        if (buttonToSelectOnExit != null)
            buttonToSelectOnExit.Select();
        gameObject.SetActive(false);
    }
}
