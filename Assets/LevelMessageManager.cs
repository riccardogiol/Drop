using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMessageManager : MonoBehaviour
{
    [Header("Personalizable parameters")]
    public int levelCode;
    int stageCode = 1;
    public string TrophyName;
    [TextArea]
    public string TrophyDescription;
    [TextArea]
    public string TrophyDescriptionBurnt = "The XXX has not been rescued yet";
    public Sprite TrophyGreenSprite;
    [Header("Change the burnt image in the image box to regulate size and position")]

    [Header("Fixed parameters")]
    public Text TrophyNameBox;
    public Text TrophyDescriptionBox;
    public Image TrophySpriteRenderer;
    public Button continueButton;
    public Text LevelTitle;

    void Start()
    {
        if ((PlayerPrefs.GetInt("Lvl" + levelCode, 0) == 0) && (PlayerPrefs.GetInt("LastStageCompleted", 0) > 0))
            continueButton.interactable = true;
        else
            continueButton.interactable = false;

        LevelTitle.text = "Level " + levelCode;
        
        if (PlayerPrefs.GetInt("Lvl" + levelCode, 0) == 1)
        {
            TrophyNameBox.text = TrophyName;
            TrophyDescriptionBox.text = TrophyDescription;
            TrophySpriteRenderer.sprite = TrophyGreenSprite;
        } else {
            TrophyDescriptionBox.text = TrophyDescriptionBurnt;
        }

    }

    public void SetMessage(int sCode)
    {
        stageCode = sCode;
        transform.Find("StagesText").GetComponent<Text>().text = "Stage " + sCode;
    }

    public void StartAction()
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-" + stageCode);
    }

    public void ContinueAction()
    {
        PlayerPrefs.SetInt("LastLevelPlayed", levelCode);
        FindObjectOfType<AudioManager>().Play("SelectSound");
        SceneManager.LoadScene("Stage" + levelCode + "-" + (PlayerPrefs.GetInt("LastStageCompleted", 0) + 1));
    }
}
