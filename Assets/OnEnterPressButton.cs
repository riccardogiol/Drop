using UnityEngine;
using UnityEngine.UI;

public class OnEnterPressButton : MonoBehaviour
{
    public Button button;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Player":
            button.onClick.Invoke();
            break;
        }
    }
}
