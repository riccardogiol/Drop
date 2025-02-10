using UnityEngine;

public class LevelTileManager : MonoBehaviour
{
    public int unlockingLvl = 0;
    public int codeLvl = 1;

    string unlockingLvlname = "Lvl1";
    string codeLvlname = "Lvl1";
    
    public StageSpotManager stageSpotManager;
    public ChangeAspect decoration;
    public GameObject bossGO;
    public GameObject waterTrophyGO;
    public GameObject SmokyCloudParent;
    public LevelTileDecorationManager levelTileDecorationManager;

    PlayerMovementPath movementPath;

    MapMessageManager messageManager;

    void Start()
    {
        unlockingLvlname = "Lvl" + unlockingLvl;
        codeLvlname = "Lvl" + codeLvl;
        movementPath = FindFirstObjectByType<PlayerMovementPath>();
        messageManager = FindFirstObjectByType<MapMessageManager>();

        stageSpotManager.lvlCode = codeLvl;

        if (PlayerPrefs.GetInt("LastLevelPlayed", 0) == codeLvl)
        {
            FindFirstObjectByType<PlayerMovementPath>().transform.position = stageSpotManager.GetStageSpot(PlayerPrefs.GetInt("LastStagePlayed", 1));
        }

        if (PlayerPrefs.GetInt(unlockingLvlname, 0) == 1)
        {
            
            SmokyCloudParent.SetActive(false);
            if (PlayerPrefs.GetInt(codeLvlname, 0) == 1)
            {
                if (decoration != null)
                    decoration.SetGreenSprite(false);
                if (waterTrophyGO != null)
                    waterTrophyGO.SetActive(true);
                if (bossGO != null)
                    bossGO.SetActive(false);
                levelTileDecorationManager.SetGreenValue(1f);
                stageSpotManager.ColorStageSpots(100);
                stageSpotManager.ActivateStageSpots(100);

            } else {
                stageSpotManager.ColorStageSpots(PlayerPrefs.GetInt("LastStageCompleted", 0));
                stageSpotManager.ActivateStageSpots(PlayerPrefs.GetInt("LastStageCompleted", 0));
                levelTileDecorationManager.SetGreenValue(PlayerPrefs.GetInt("LastStageCompleted", 0)/5f);
            }
        } else {
            stageSpotManager.DisableAllStageSpots();
            levelTileDecorationManager.SetGreenValue(0f);
            if (decoration != null)
                decoration.ColorAdjustment(0f, 0.2f);
        }
    }

    public void MoveOnThisTile()
    {
        if (MapMessageManager.messageOnScreen)
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
