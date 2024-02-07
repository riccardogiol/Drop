using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    public Button continueButton;
    bool musicState;

    void Start()
    {
        if ((PlayerPrefs.GetInt("Lvl1", 0) == 1) || PlayerPrefs.GetInt("LastStageCompleted", 0) > 0)
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        
        musicState = PlayerPrefs.GetInt("MusicState", 1) == 1;
        UpdateMusicToggleText();
        
    }

    public void NewGame()
    {
        // add disclaimer in the case the game is already started
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("MusicState", musicState?1:0);
        PlayerPrefs.SetInt("Lvl0", 1);
        PlayerPrefs.SetInt("FromMainMenu", 1);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("WorldMap");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("FromMainMenu", 1);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("WorldMap");
    }

    public void ToggleMusic()
    {
        if (musicState)
        {
            FindFirstObjectByType<AudioManager>().SetVolume(0f);
            musicState = false;
            PlayerPrefs.SetInt("MusicState", 0);
        } else {
            FindFirstObjectByType<AudioManager>().SetVolume(0.2f);
            musicState = true;
            PlayerPrefs.SetInt("MusicState", 1);
        }
        UpdateMusicToggleText();
    }

    void UpdateMusicToggleText()
    {
        Transform auxTrans = transform.Find("ToggleMusicButton");
        auxTrans = auxTrans.transform.Find("Text");
        if (musicState)
            auxTrans.GetComponent<Text>().text = "Music: off";
        else
            auxTrans.GetComponent<Text>().text = "Music: on";

    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("SelectSound");
        Application.Quit();
    }
}
