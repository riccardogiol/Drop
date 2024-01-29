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
    public StageSpotManager stageSpotManager;
    public SpriteRenderer buttonHighlighter;
    public ChangeAspect decoration;
    public ParticleSystem smokeEffect;
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
                decoration.SetGreenSprite();
                stageSpotManager.ColorStageSpots(100);
                buttonHighlighter.color = new Color(46f/255, 76f/255, 0f/255);

            } else {
                stageSpotManager.ColorStageSpots(PlayerPrefs.GetInt("LastStageCompleted", 0));
                smokeEffect.Play();
                buttonHighlighter.color = new Color(230f/255, 150f/255, 36f/255);
                buttonHighlighter.GetComponent<Animator>().SetBool("IsHighlighted", true);
            }
        } else {  
            button.interactable = false;
            smokeEffect.Play();
            SmokyCloudParent.SetActive(true);
        }
    }

    public void MoveOnThisTile()
    {
        movementPath.NewTarget(dropSpot.transform.position);
        dropSpot.GetComponent<CircleCollider2D>().enabled = true;
        FindFirstObjectByType<MapMoveCamera>().Exit();
    }

    public void StartLevel()
    {
        messageManager.ShowLevelMessage(codeLvl);
    }
}
