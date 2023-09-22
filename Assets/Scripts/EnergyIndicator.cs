using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicator : MonoBehaviour
{
    int valueToDisplay = 0;
    public Text text;
    public PickFlame flame;
    public PickWaterdrop waterdrop;

    public void ShowEnergy()
    {
        if (flame != null)
            valueToDisplay = flame.energy;
        else if (waterdrop != null)
            valueToDisplay = waterdrop.energy;
        else
            valueToDisplay = 0;
        SetText();
    }

    void SetText()
    {
        text.enabled = true;
        if (flame != null)
            text.text = "-" + valueToDisplay;
        else
            text.text = "+" + valueToDisplay;
    }

    public void HideEnergy()
    {
        text.enabled = false;
    }
}
