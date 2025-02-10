using UnityEngine;

public class OnEnterCallDestroy : MonoBehaviour
{
    public DestroyAndNeverShowAgain destroyAndNeverShowAgain;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            if (destroyAndNeverShowAgain ==  null)
            {
                gameObject.SetActive(false);
                return;
            }
            destroyAndNeverShowAgain.DestroyForever();
            break;
        }
    }
}
