using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicator : MonoBehaviour
{
    int valueToDisplay = 0;
    public Text text;
    public Image image;
    public PickFlame flame;
    public PickWaterdrop waterdrop;
    public PickSuperdrop superdrop;
    public EnemyHealth enemyHealth;
    public SparklerCharge sparklerCharge;
    public EnemyAIPatrolMovement patrolMovement;
    public EnemyAIChasingMovement chasingMovement;

    public Sprite patrolIcon;
    public Sprite chasingIcon;

    public void ShowEnergy()
    {
        if (flame != null)
            valueToDisplay = flame.energy;
        else if (waterdrop != null)
            valueToDisplay = waterdrop.energy;
        else if (superdrop != null)
            valueToDisplay = superdrop.energy;
        else if (enemyHealth != null)
            valueToDisplay = enemyHealth.currentHealth;
        else if (sparklerCharge != null)
            valueToDisplay = sparklerCharge.maxCharge - sparklerCharge.currentCharge;
        else
            valueToDisplay = 0;
        SetText();
        if (image != null)
            SetImage();

        if (chasingMovement != null)
            chasingMovement.ShowPath();
        if (patrolMovement != null)
            patrolMovement.ShowPath();
    }

    void SetText()
    {
        text.enabled = true;
        if (waterdrop != null || superdrop != null)
            text.text = "+" + valueToDisplay;
        else
            text.text = "-" + valueToDisplay;
    }

    void SetImage()
    {
        image.sprite = chasingIcon;
        if (patrolMovement != null)
        {
            if (patrolMovement.enabled)
                image.sprite = patrolIcon;
        }
        image.enabled = true;
    }

    public void HideEnergy()
    {
        text.enabled = false;
        if (image != null)
            image.enabled = false;
            
        if (chasingMovement != null)
            chasingMovement.HidePath();
            
        if (patrolMovement != null)
            patrolMovement.HidePath();
    }
}
