using UnityEngine;
using UnityEngine.UI;

public class LevelTileManagerNew : MonoBehaviour
{
    public int unlockingLvl = 0;
    public int codeLvl = 1;

    string unlockingLvlname = "Lvl1";
    string codeLvlname = "Lvl1";
    
    public Button button;
    public StageSpotManager stageSpotManager;
    public SpriteRenderer buttonHighlighter;
    public ChangeAspect decoration;
    public ParticleSystem smokeEffect;
    public GameObject SmokyCloudParent;
    public WorldTileDecorationManager worldTileDecorationManager;

    PlayerMovementPath movementPath;

    MapMessageManager messageManager;

    void Start()
    {
        unlockingLvlname = "Lvl" + unlockingLvl;
        codeLvlname = "Lvl" + codeLvl;
        movementPath = FindFirstObjectByType<PlayerMovementPath>();
        messageManager = FindFirstObjectByType<MapMessageManager>();

        if (PlayerPrefs.GetInt("LastLevelPlayed", 0) == codeLvl)
        {
            FindFirstObjectByType<PlayerMovementPath>().transform.position = stageSpotManager.GetStageSpot(PlayerPrefs.GetInt("LastStagePlayed", 1));
            button.Select();
        }

        if (PlayerPrefs.GetInt(unlockingLvlname, 0) == 1)
        {
            button.interactable = true;
            if (PlayerPrefs.GetInt(codeLvlname, 0) == 1)
            {
                decoration.SetGreenSprite();
                worldTileDecorationManager.SetGreenValue(1f);
                stageSpotManager.ColorStageSpots(100);
                stageSpotManager.ActivateStageSpots(100);
                buttonHighlighter.color = new Color(46f/255, 76f/255, 0f/255);

            } else {
                stageSpotManager.ColorStageSpots(PlayerPrefs.GetInt("LastStageCompleted", 0));
                stageSpotManager.ActivateStageSpots(PlayerPrefs.GetInt("LastStageCompleted", 0));
                worldTileDecorationManager.SetGreenValue(PlayerPrefs.GetInt("LastStageCompleted", 0)/5f);
                smokeEffect.Play();
                buttonHighlighter.color = new Color(230f/255, 150f/255, 36f/255);
                buttonHighlighter.GetComponent<Animator>().SetBool("IsHighlighted", true);
            }
        } else {  
            button.interactable = false;
            smokeEffect.Play();
            SmokyCloudParent.SetActive(true);
            stageSpotManager.ColorStageSpots(-100);

        }
    }

    public void MoveOnThisTile()
    {
        if (FindFirstObjectByType<MapMessageManager>().messageOnScreen)
            return;
        Debug.Log(transform.name);
        Vector3 newPos;
        if (PlayerPrefs.GetInt("Lvl" + codeLvl, 0) == 0)
        {
            newPos = stageSpotManager.GetStageSpot(PlayerPrefs.GetInt("LastStageCompleted", 1) + 1);
            stageSpotManager.ActivateStageSpot(PlayerPrefs.GetInt("LastStageCompleted", 1) + 1);
        } else {
            newPos = stageSpotManager.GetStageSpot(1);
            stageSpotManager.ActivateStageSpot(1);
        }
        movementPath.NewTarget(newPos);
        FindFirstObjectByType<MapMoveCamera>().Exit();
    }

    public void StartLevel(int stageCode = 1)
    {
        messageManager.ShowLevelMessage(codeLvl, stageCode);
    }
}
