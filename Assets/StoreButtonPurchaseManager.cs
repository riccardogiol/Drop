using UnityEngine;
using UnityEngine.UI;

public class StoreButtonPurchaseManager : MonoBehaviour
{
    public Button button;
    public Image buttonImage;
    public Text text;

    public Sprite demoSprite, fullVersionSprite;

    public string price;

    string localizedText = "";
    
    public void UpdateButtonGFX()
    {
        if (text == null || button == null || buttonImage == null)
            return;
        if (PlayerPrefs.GetInt("FullVersion", 0) == 1)
        {
            buttonImage.sprite = fullVersionSprite;
            localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.main.fv");
            if (localizedText == null)
                localizedText = "FULL\nVERSION";
            text.text = localizedText.ToUpper();
            text.color = Color.black;
            button.enabled = false;
        } else
        {
            buttonImage.sprite = demoSprite;
            localizedText = SingletonLocalizationManager.instance.GetComponent<LocalizationManager>().Get("menu.main.buy_fv");
            if (localizedText == null)
                localizedText = "BUY FULL VERSION";
            text.text = localizedText.ToUpper() + "\n" + price;
            text.color = Color.white;
            button.enabled = true;
        }
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }
}
