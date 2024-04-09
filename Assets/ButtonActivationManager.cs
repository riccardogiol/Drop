using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationManager : MonoBehaviour
{
    Button button;
    Image image;
    public Sprite lockedSprite;
    public string unlockingCode;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        
        if (PlayerPrefs.GetInt(unlockingCode, 0) == 0)
        {
            button.interactable = false;
            image.sprite = lockedSprite;
        }
        
    }
}
