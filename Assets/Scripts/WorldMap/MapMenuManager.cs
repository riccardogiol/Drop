using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MapMenuManager : MonoBehaviour
{
    bool gamepadInput = false;
    
    Vector3 lastMousePos;
    void Awake()
    {
        FindObjectOfType<AudioManager>().ResetSounds();
        FindObjectOfType<AudioManager>().Play("OpeningMusic");
        FindObjectOfType<AudioManager>().StopStageMusic();
        FindObjectOfType<AudioManager>().LowFilerExit();
        
        Cursor.visible = true;
        lastMousePos = Vector3.one;
        if (Input.mousePosition != null)
           lastMousePos = Input.mousePosition;
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            PlayerPrefs.SetInt("ChallengeDisabled", 1);
    }

     void Update()
    {
        if (!Cursor.visible && Input.mousePosition != lastMousePos)
        {
            Cursor.visible = true;
            lastMousePos = Input.mousePosition;
        }
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
