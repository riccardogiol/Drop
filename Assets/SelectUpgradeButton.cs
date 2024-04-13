using UnityEngine;

public class SelectUpgradeButton : MonoBehaviour
{
    ButtonActivationManager buttonActivationManager;
    public string upgradeTitle;
    public string upgradeDescription;
    public int upgradePrice;
    public Sprite powerSprite;
    public Sprite upgradeSprite;

    StoreDisplayManager storeDisplayManager;

    void Awake()
    {
        buttonActivationManager = GetComponent<ButtonActivationManager>();
        storeDisplayManager = FindFirstObjectByType<StoreDisplayManager>();        
    }

    public void Select()
    {
        storeDisplayManager.ShowUpgradeDescription(buttonActivationManager.buttonKeyCode, upgradeTitle, upgradeDescription, upgradePrice, powerSprite, upgradeSprite, buttonActivationManager);
    }
}
