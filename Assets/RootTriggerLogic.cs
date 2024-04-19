using UnityEngine;

public class RootTriggerLogic : MonoBehaviour
{
    public ChangeAspect changeAspect;
    
    [System.NonSerialized]
    public bool reactOnWater;

    void Start()
    {
        reactOnWater = changeAspect.reactOnWater;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!reactOnWater)
            return;
        switch(other.tag)
        {
            case "Waterbullet":
            case "Wave":
                changeAspect.SetGreenSprite();
                break;
        }
    }

    public void SetGreenSprite(bool playWater = true, bool playLeaves = true)
    {
        changeAspect.SetGreenSprite(playWater, playLeaves);
    }

    public void SetBurntSprite(bool playLeaves = true)
    {
        changeAspect.SetBurntSprite(playLeaves);
    }

}
