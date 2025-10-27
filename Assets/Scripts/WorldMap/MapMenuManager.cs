using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMenuManager : MonoBehaviour
{
    void Awake()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().Stop("BackgroundMusic");
    }
    
    public void GoMainMenu()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
        PlayerPrefs.SetInt("FromWorldMap", 1);
        SceneManager.LoadScene("MainMenu");
    }
}
