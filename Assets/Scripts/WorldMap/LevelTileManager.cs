using UnityEngine;
using UnityEngine.UI;

public class LevelTileManager : MonoBehaviour
{
    public string unlockingLvl = "Lvl1";
    public string codeLvl = "Lvl1";
    
    public Button button;
    public SpriteRenderer dropSpot;
    public ChangeAspect decoration;
    // SmokyCloudRenderer

    PlayerMovementPath movementPath;

    void Start()
    {
        movementPath = FindFirstObjectByType<PlayerMovementPath>();

        if (PlayerPrefs.GetInt(unlockingLvl, 0) == 1)
        {
            button.interactable = true;
            // disable clouds
            if (PlayerPrefs.GetInt(codeLvl, 0) == 1)
            {
                dropSpot.color = new Color(104.0f/255, 189.0f/255, 225.0f/255);
                decoration.SetGreenSprite();
            } else {
                dropSpot.color = new Color(241.0f/255, 154.0f/255, 40.0f/255);
            }
        } else {  
            button.interactable = false;
            //activate clouds
        }
    }

    public void MoveOnThisTile()
    {
        movementPath.NewTarget(dropSpot.transform.position);
        dropSpot.GetComponent<CircleCollider2D>().enabled = true;
    }

    public void StartLevel()
    {
        Debug.Log("Starting level" + codeLvl);
    }
}
