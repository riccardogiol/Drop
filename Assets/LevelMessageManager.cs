using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMessageManager : MonoBehaviour
{
    [Header("Personalizable parameters")]
    public int levelCode;
    public string TrophyName;
    [TextArea]
    public string TrophyDescription;
    public Sprite TrophyGreenSprite;

    [Header("Fixed parameters")]
    public Text TrophyNameBox;
    public Text TrophyDescriptionBox;
    public Image TrophySpriteRenderer;
    public Button continueButton;

    void Start()
    {
        if ((PlayerPrefs.GetInt("Lvl" + levelCode, 0) == 0) && (PlayerPrefs.GetInt("LastStageCompleted", 0) > 0))
            continueButton.interactable = true;
        else
            continueButton.interactable = false;
        
        if (PlayerPrefs.GetInt("Lvl" + levelCode, 0) == 1)
        {
            TrophyNameBox.text = TrophyName;
            TrophyDescriptionBox.text = TrophyDescription;
            TrophySpriteRenderer.sprite = TrophyGreenSprite;
        }

    }

    public void StartAction()
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-1");
    }

    public void ContinueAction()
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-" + (PlayerPrefs.GetInt("LastStageCompleted", 0) + 1));
    }
}
