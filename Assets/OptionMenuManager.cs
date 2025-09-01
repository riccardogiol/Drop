using UnityEngine;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;

    void Start()
    {
        UpdateMusicSlider();
        UpdateSoundSlider();
        UpdateHUDToggleText();
    }

    public void AdjustMusicVolume(float value)
    {
        FindFirstObjectByType<AudioManager>().SetMusicVolume(value / 5.0f);
        PlayerPrefs.SetFloat("MusicVolume", value / 5.0f);
    }

    void UpdateMusicSlider()
    {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1.0f) * 5);
    }

    public void AdjustSoundVolume(float value)
    {
        FindFirstObjectByType<AudioManager>().SetSoundVolume(value / 5.0f);
        PlayerPrefs.SetFloat("SoundVolume", value / 5.0f);
    }

    void UpdateSoundSlider()
    {
        soundSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SoundVolume", 1.0f) * 5);
    }
    
    public void ToggleHUD()
    {
        if (PlayerPrefs.GetInt("ShowButtonHint", 0) == 0)
            PlayerPrefs.SetInt("ShowButtonHint", 1);
        else
            PlayerPrefs.SetInt("ShowButtonHint", 0);
        UpdateHUDToggleText();
    }

    void UpdateHUDToggleText()
    {
        Transform auxTrans = transform.Find("ToggleButtonHints");
        auxTrans = auxTrans.transform.Find("Text");
        if (PlayerPrefs.GetInt("ShowButtonHint", 0) == 1)
            auxTrans.GetComponent<Text>().text = "HUD: off";
        else
            auxTrans.GetComponent<Text>().text = "HUD: on";
    }
}
