using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationOnPP : MonoBehaviour
{
    public string playerPrefsKey = "";
    public int activeOn = 0;

    Button button;

    void Awake()
    {
      button = GetComponent<Button>();  
    }

    void Start()
    {
        UpdateButtonInteraction();
    }

    public void UpdateButtonInteraction()
    {
        if (PlayerPrefs.GetInt(playerPrefsKey, 0) == activeOn)
           button.interactable = true;
        else
           button.interactable = false;
    }
}
