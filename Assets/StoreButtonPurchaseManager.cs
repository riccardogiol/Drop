using UnityEngine;
using UnityEngine.UI;

public class StoreButtonPurchaseManager : MonoBehaviour
{
    public Button button;
    public Image buttonImage;
    public Text text;

    public Sprite demoSprite, fullVersionSprite;

    public string price;
    
    public void UpdateButtonGFX()
    {
        if (text == null || button == null || buttonImage == null)
            return;
        if (PlayerPrefs.GetInt("FullVersion", 0) == 1)
        {
            buttonImage.sprite = fullVersionSprite;
            text.text = "FULL\nVERSION"; // localizza controlla in messaggio scelta difficoltà per farlo embedded
            text.color = Color.black;
            button.enabled = false;
        } else
        {
            buttonImage.sprite = demoSprite;
            text.text = "BUY FULL VERSION\n" + price; // localizza
            text.color = Color.white;
            button.enabled = true;
        }
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }
}
