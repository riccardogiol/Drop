using UnityEngine;
using UnityEngine.UI;

public class StoreDisplayManager : MonoBehaviour
{
    public Image powerImage;
    public Image upgradeImage;
    public Text title;
    public Text description;
    public Button purchaseButton;
    public Text purchaseButtonText;
    public Text priceText;
    int price;
    string upgradeCode;
    ButtonActivationManager buttonActivationManager;

    CoinCounterUpdate coinCounterUpdate;

    void Awake()
    {
        coinCounterUpdate = FindFirstObjectByType<CoinCounterUpdate>();
    }

    public void ShowUpgradeDescription(string upgradeKeyCode, string upgradeTitleKey, string upgradeDescriptionKey, int upgradePrice, Sprite powerSprite, Sprite upgradeSprite, ButtonActivationManager bam)
    {
        powerImage.sprite = powerSprite;
        if (powerSprite != null)
            powerImage.color = new Color(1, 1, 1, 1);
        else
            powerImage.color = new Color(1, 1, 1, 0);
        upgradeImage.sprite = upgradeSprite;
        if (upgradeSprite != null)
            upgradeImage.color = new Color(1, 1, 1, 1);
        else
            upgradeImage.color = new Color(1, 1, 1, 0);

        string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(upgradeTitleKey);
        if (localizedText != null)
            title.text = localizedText;
        else
            title.text = upgradeTitleKey;
        title.text = title.text.ToUpper();

        localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get(upgradeDescriptionKey);
        if (localizedText != null)
            description.text = localizedText;
        else
            description.text = upgradeTitleKey;

        priceText.text = "" + upgradePrice;
        price = upgradePrice;
        upgradeCode = upgradeKeyCode;
        buttonActivationManager = bam;

        if (PlayerPrefs.GetInt(upgradeKeyCode, 0) == 0)
        {
            if (PlayerPrefs.GetInt("CoinAmount", 0) >= price)
            {
                localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.unlock");
                if (localizedText != null)
                    purchaseButtonText.text = localizedText;
                else
                    purchaseButtonText.text = "Unlock";
                purchaseButton.interactable = true;
            }
            else
            {
                localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.no_exp");
                if (localizedText != null)
                    purchaseButtonText.text = localizedText;
                else
                    purchaseButtonText.text = "no exp";
                purchaseButton.interactable = false;
            }
        }
        else
        {
            purchaseButton.interactable = false;
            localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.active");
            if (localizedText != null)
                purchaseButtonText.text = localizedText;
            else
                purchaseButtonText.text = "Active";
        }
        purchaseButtonText.text = purchaseButtonText.text.ToUpper();
    }

    public void PurchaseUpgrade()
    {
        if (PlayerPrefs.GetInt("CoinAmount", 0) >= price)
        {
            PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) - price);
            PlayerPrefs.SetInt(upgradeCode,1);
            purchaseButton.interactable = false;
            string localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.world.active");
            if (localizedText != null)
                purchaseButtonText.text = localizedText;
            else
                purchaseButtonText.text = "Active";
            coinCounterUpdate.Refresh();
            buttonActivationManager.UpdateGFX();
        }
    }
}
