using UnityEngine;
using UnityEngine.UI;

public class ChallengeInfo : MonoBehaviour
{
    public Text textComp;
    public Image medalGFX;
    public Color disabledColor;
    public Color potentialColor;
    public Color wonColor;

    public void WriteText(string info)
    {
        textComp.text = info;
    }

    /// <summary>
    /// State is 0 for disable, 1 for potential and 2 for won
    /// </summary>
    public void SetMedalState(int state)
    {
        switch (state)
        {
            case 2:
                medalGFX.color = wonColor;
                break;
            case 1:
                medalGFX.color = potentialColor;
                break;
            default:
                medalGFX.color = disabledColor;
                break;
        }
    }

    public void SetMedalGFX(string medalCode)
    {
        Sprite medalSprite = Resources.Load<Sprite>("Sprites/Elements/" + medalCode);
        if (medalSprite != null)
            medalGFX.sprite = medalSprite;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
