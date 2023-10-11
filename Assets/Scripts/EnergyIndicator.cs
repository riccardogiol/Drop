using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicator : MonoBehaviour
{
    int valueToDisplay = 0;
    public Text text;
    public Image image;
    public PickFlame flame;
    public PickWaterdrop waterdrop;
    public EnemyHealth enemyHealth;

    public void ShowEnergy()
    {
        if (flame != null)
            valueToDisplay = flame.energy;
        else if (waterdrop != null)
            valueToDisplay = waterdrop.energy;
        else if (enemyHealth != null)
            valueToDisplay = enemyHealth.currentHealth; // display current and max!
        else
            valueToDisplay = 0;
        SetText();
    }

    void SetText()
    {
        text.enabled = true;
        if (waterdrop != null)
            text.text = "+" + valueToDisplay;
        else
            text.text = "-" + valueToDisplay;
        if (image != null)
            image.enabled = true;
    }

    public void HideEnergy()
    {
        text.enabled = false;
        if (image != null)
            image.enabled = false;
    }
}
