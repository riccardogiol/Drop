using UnityEngine;
using UnityEngine.UI;

public class SelectCollectionButton : MonoBehaviour
{
    public string elementKey = "bee";
    public string unlockingCode = "Lvl1";
    public bool isEnemy = false;
    public TextLocalizer textLocalizer;

    ButtonActivationManager buttonActivationManager;
    string elementTitleKey;
    string elementDescriptionKey;
    int upgradePrice;
    Sprite elementSprite;
    Sprite behaviourSymbolSprite;
    StoreDisplayManager storeDisplayManager;

    void Awake()
    {
        string subFolder = "trophy.";
        if (isEnemy)
            subFolder = "enemy.";
        buttonActivationManager = GetComponent<ButtonActivationManager>();
        storeDisplayManager = FindFirstObjectByType<StoreDisplayManager>();

        elementTitleKey = "content." + subFolder + elementKey + ".name";
        elementDescriptionKey = "content." + subFolder + elementKey + ".description";

        elementSprite = Resources.Load<Sprite>("Sprites/Elements/" + elementKey);
        behaviourSymbolSprite = Resources.Load<Sprite>("Sprites/Elements/" + elementKey + "_BS");

        buttonActivationManager.unlockingCode = unlockingCode;
        buttonActivationManager.buttonKeyCode = unlockingCode;

        if (PlayerPrefs.GetInt(unlockingCode, 0) == 0)
            textLocalizer.key = "menu.general.unknown";
        else
            textLocalizer.key = "content." + subFolder + elementKey + ".name";
        textLocalizer.Localize();
        Button button = GetComponent<Button>();
        if (button != null)
        {
            var nav = new Navigation{ mode = Navigation.Mode.Automatic };
            button.navigation = nav;
        }
    }
    
    public void Select()
    {
        FindObjectOfType<AudioManager>().Play("ClickIn");
        storeDisplayManager.ShowUpgradeDescription(buttonActivationManager.buttonKeyCode, elementTitleKey, elementDescriptionKey, upgradePrice, elementSprite, behaviourSymbolSprite, buttonActivationManager);
    }
}
