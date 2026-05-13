using UnityEngine;
using UnityEngine.UI;

public class StoreButtonPurchaseManager : MonoBehaviour
{
    public Button button;
    public Image buttonImage;
    public Text text;

    public Sprite demoSprite, fullVersionSprite;
    
    public void UpdateButtonGFX()
    {
        if (text == null || button == null || buttonImage == null)
            return;
        if (PlayerPrefs.GetInt("FullVersion", 0) == 1)
        {
            buttonImage.sprite = fullVersionSprite;
            text.text = "Full Version"; // localizza?
        } else
        {
            buttonImage.sprite = demoSprite;
            text.text = "BUY FULL VERSION"; // localizza?
        }
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }
}
