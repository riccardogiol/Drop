using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationManager : MonoBehaviour
{
    Button button;
    Image image;
    public Sprite lockedSprite;
    Sprite unlockedSprite;
    public string unlockingCode;
    public ButtonActivationManager previousUpgrade;
    bool locked = false;

    public bool activable = false;
    public Image trail;
    public string buttonKeyCode;
    public Sprite activatedSpot;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        unlockedSprite = image.sprite;
        
        bool condition;
        if (previousUpgrade != null)
            condition = PlayerPrefs.GetInt(unlockingCode, 0) == 0 || PlayerPrefs.GetInt(previousUpgrade.buttonKeyCode, 0 ) == 0;
        else 
            condition = PlayerPrefs.GetInt(unlockingCode, 0) == 0;

        if (condition)
        {
            if (button != null)
                button.interactable = false;
            image.color = new Color(0.2f, 0.2f, 0.2f);
            //image.sprite = lockedSprite;
            locked = true;
        }

        UpdateGFX();
    }

    void Update()
    {
        if (locked)
        {
            bool condition;
            if (previousUpgrade != null)
                condition = PlayerPrefs.GetInt(unlockingCode, 0) == 1 && PlayerPrefs.GetInt(previousUpgrade.buttonKeyCode, 0 ) == 1;
            else 
                condition = PlayerPrefs.GetInt(unlockingCode, 0) == 1;

            if (condition)
            {
                image.sprite = unlockedSprite;
                image.color = new Color(1, 1, 1);
                if (button != null)
                    button.interactable = true;
                locked = false;
                UpdateGFX();
            }
        } else 
        {
            bool condition;
            if (previousUpgrade != null)
                condition = PlayerPrefs.GetInt(unlockingCode, 0) == 0 || PlayerPrefs.GetInt(previousUpgrade.buttonKeyCode, 0 ) == 0;
            else 
                condition = PlayerPrefs.GetInt(unlockingCode, 0) == 0;
            if (condition)
            {
                if (button != null)
                    button.interactable = false;
                image.sprite = lockedSprite;
                locked = true;
                UpdateGFX();
            }
        }
    }

    public void UpdateGFX()
    {
        if (!activable)
            return;
        
        bool condition;
        if (previousUpgrade != null)
            condition = PlayerPrefs.GetInt(unlockingCode, 0) == 0 || PlayerPrefs.GetInt(previousUpgrade.buttonKeyCode, 0 ) == 0;
        else 
            condition = PlayerPrefs.GetInt(unlockingCode, 0) == 0;

        if (condition)
        {
            if (trail != null)
                trail.color = new Color(0.3f, 0.3f, 0.3f);
            image.color = new Color(0.7f, 0.7f, 0.7f);
        } else {
            if (trail != null)
                trail.color = new Color(214f/255, 196f/255, 66f/255);
            image.color = new Color(0.86f, 0.82f, 0.56f);
        }
        
        if (PlayerPrefs.GetInt(buttonKeyCode, 0) == 1)
        {
            if (trail != null)
                trail.color = new Color(157f/255, 214f/255, 66f/255);
            image.color = new Color(0.54f, 0.70f, 0.32f);
            if (activatedSpot != null)
                image.sprite = activatedSpot;
        }
    }
}
