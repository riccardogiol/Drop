using UnityEngine;

public class OnEnterSetActive : MonoBehaviour
{
    public GameObject obj;
    public bool setActive = false;

    
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            obj.SetActive(setActive);
            break;
        }
    }
}
