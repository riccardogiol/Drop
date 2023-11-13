using UnityEngine;
using UnityEngine.UI;

public class LevelTileManager : MonoBehaviour
{
    
    public int unlockingLvl = 0;
    public int codeLvl = 1;

    string unlockingLvlname = "Lvl1";
    string codeLvlname = "Lvl1";
    
    public Button button;
    public SpriteRenderer dropSpot;
    public ChangeAspect decoration;
    public GameObject SmokyCloudParent;

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
            movementPath.transform.position = dropSpot.transform.position;
        }

        if (PlayerPrefs.GetInt(unlockingLvlname, 0) == 1)
        {
            button.interactable = true;
            if (PlayerPrefs.GetInt(codeLvlname, 0) == 1)
            {
                dropSpot.color = new Color(104.0f/255, 189.0f/255, 225.0f/255);
                decoration.SetGreenSprite();
            } else {
                dropSpot.color = new Color(241.0f/255, 154.0f/255, 40.0f/255);
            }
        } else {  
            button.interactable = false;
            SmokyCloudParent.SetActive(true);
        }
    }

    public void MoveOnThisTile()
    {
        movementPath.NewTarget(dropSpot.transform.position);
        dropSpot.GetComponent<CircleCollider2D>().enabled = true;
    }

    public void StartLevel()
    {
        messageManager.ShowLevelMessage(codeLvl);
    }
}
