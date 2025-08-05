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

    public void ShowUpgradeDescription(string upgradeKeyCode, string upgradeTitle, string upgradeDescription, int upgradePrice, Sprite powerSprite, Sprite upgradeSprite, ButtonActivationManager bam)
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
        title.text = upgradeTitle;
        description.text = upgradeDescription;
        priceText.text = "" + upgradePrice;
        price = upgradePrice;
        upgradeCode = upgradeKeyCode;
        buttonActivationManager = bam;

        if (PlayerPrefs.GetInt(upgradeKeyCode, 0) == 0)
        {
            if (PlayerPrefs.GetInt("CoinAmount", 0) >= price)
            {
                purchaseButtonText.text = "Unlock";
                purchaseButton.interactable = true;
            } else {
                purchaseButtonText.text = "no exp";
                purchaseButton.interactable = false;
            }
        } else {
            purchaseButton.interactable = false;
            purchaseButtonText.text = "Active";
        }
    }

    public void PurchaseUpgrade()
    {
        if (PlayerPrefs.GetInt("CoinAmount", 0) >= price)
        {
            PlayerPrefs.SetInt("CoinAmount", PlayerPrefs.GetInt("CoinAmount", 0) - price);
            PlayerPrefs.SetInt(upgradeCode,1);
            purchaseButton.interactable = false;
            purchaseButtonText.text = "Active";
            coinCounterUpdate.Refresh();
            buttonActivationManager.UpdateGFX();
        }
    }
}
