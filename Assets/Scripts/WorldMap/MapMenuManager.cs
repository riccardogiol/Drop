using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MapMenuManager : MonoBehaviour
{
    bool gamepadInput = false;
    void Awake()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().StopStageMusic();
        FindObjectOfType<AudioManager>().LowFilerExit();
    }

     void Update()
    {
        if (Gamepad.current != null)
            gamepadInput = Gamepad.current.startButton.wasPressedThisFrame;
        if (Input.GetKeyDown(KeyCode.Escape) || gamepadInput)
            if (!MapMessageManager.messageOnScreen)
                 GoMainMenu();
    }
    
    public void GoMainMenu()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
        PlayerPrefs.SetInt("FromWorldMap", 1);
        SceneManager.LoadScene("MainMenu");
    }
}
