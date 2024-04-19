using UnityEngine;

public class BranchTriggerLogic : MonoBehaviour
{
    public ChangeAspect changeAspect;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Player":
                changeAspect.SetTransparancy(true);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Player":
                changeAspect.SetTransparancy(false);
                break;
        }
    }

}
