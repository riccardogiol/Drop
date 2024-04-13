using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationManager : MonoBehaviour
{
    Button button;
    Image image;
    public Sprite lockedSprite;
    Sprite unlockedSprite;
    public string unlockingCode;
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
        
        if (PlayerPrefs.GetInt(unlockingCode, 0) == 0)
        {
            if (button != null)
                button.interactable = false;
            image.sprite = lockedSprite;
            locked = true;
        }

        UpdateGFX();
    }

    void Update()
    {
        if (locked)
        {
            if (PlayerPrefs.GetInt(unlockingCode, 0) == 1)
            {
                image.sprite = unlockedSprite;
                if (button != null)
                    button.interactable = true;
                locked = false;
                UpdateGFX();
            }

        }
    }

    public void UpdateGFX()
    {
        if (!activable)
            return;
        if (PlayerPrefs.GetInt(unlockingCode, 0) == 0)
        {
            trail.color = new Color(0.3f, 0.3f, 0.3f);
            image.color = new Color(0.7f, 0.7f, 0.7f);
        } else {
            trail.color = new Color(214f/255, 196f/255, 66f/255);
            image.color = new Color(0.86f, 0.82f, 0.56f);
        }
        
        if (PlayerPrefs.GetInt(buttonKeyCode, 0) == 1)
        {
            trail.color = new Color(157f/255, 214f/255, 66f/255);
            image.color = new Color(0.54f, 0.70f, 0.32f);
            image.sprite = activatedSpot;
        }
    }
}
