using UnityEngine;

public class ExitAndSwitchMessage : MonoBehaviour
{
    public MenusManager menuManager;
    public SwitchMessage switchMessage;

    public void Select()
    {
        switchMessage.Switch();
        menuManager.ExitMessage(gameObject);
    }
}
