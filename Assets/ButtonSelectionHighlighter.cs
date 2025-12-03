using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelectionHighlighter : MonoBehaviour
{
    public GameObject highligther;
    GameObject selectedButton;
    public InitializeButtonSelection ibs;

    RectTransform selectedRT;
    Vector2 p;

    public bool showOnlyOnGamepad = true;

    void Update()
    {
        selectedButton = EventSystem.current.currentSelectedGameObject;

        if (selectedButton == null)
        {
            if (ibs != null)
               ibs.Refresh();
            highligther.gameObject.SetActive(false);
            return;
        }

        if (showOnlyOnGamepad && Gamepad.current == null)
        {
            enabled = false;
            return;
        }

        highligther.gameObject.SetActive(true);

        selectedRT = selectedButton.GetComponent<RectTransform>();

        if (selectedRT != null)
        {
            p = new Vector2(selectedRT.rect.xMin, selectedRT.rect.center.y);
            highligther.transform.position = selectedRT.TransformPoint(p) + new Vector3(-50, 0, 0);
            if (selectedButton.GetComponent<Slider>() != null)
                highligther.transform.position += new Vector3(-130, 0, 0);

        }
    }
}
