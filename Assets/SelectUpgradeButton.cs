using UnityEngine;

public class SelectUpgradeButton : MonoBehaviour
{
    ButtonActivationManager buttonActivationManager;
    public string upgradeTitleKey;
    public string upgradeDescriptionKey;
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
        storeDisplayManager.ShowUpgradeDescription(buttonActivationManager.buttonKeyCode, upgradeTitleKey, upgradeDescriptionKey, upgradePrice, powerSprite, upgradeSprite, buttonActivationManager);
    }
}
