using System;
using UnityEngine;
using UnityEngine.UI;

public class ShelfButtonsManager : MonoBehaviour
{
    RectTransform rectTransform;
    public Button nextButton, previousButton;
    public RectTransform[] elements;
    public Text pageNumber;

    float buttonHeight = 80, buttonsDistance = 10, verticalSpace;
    int maxElementsToDisplay, currentPage;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        verticalSpace = rectTransform.rect.height;
        maxElementsToDisplay = (int)Math.Floor(verticalSpace / (buttonHeight + buttonsDistance));
        currentPage = 0;
        DisplayPage();
    }

    void DisplayPage()
    {
        pageNumber.text = "" + (currentPage+1);
        int lastElement = Mathf.Min(maxElementsToDisplay * (currentPage + 1), elements.Length);
        int firstElement = Math.Max(lastElement - maxElementsToDisplay, 0);
        float lastPosition = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            if (i < firstElement)
                elements[i].gameObject.SetActive(false);
            else if (i >= lastElement)
                elements[i].gameObject.SetActive(false);
            else
            {
                elements[i].anchoredPosition = new Vector2(0, lastPosition);
                lastPosition -= buttonHeight + buttonsDistance;
                elements[i].gameObject.SetActive(true);
            }
        }

        if (firstElement >= 1)
            previousButton.interactable = true;
        else
            previousButton.interactable = false;

        if (lastElement < elements.Length)
            nextButton.interactable = true;
        else
            nextButton.interactable = false;

        InitializeButtonSelection ibs = transform.parent.parent.GetComponent<InitializeButtonSelection>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        if (ibs != null)
            ibs.Refresh();
    }

    public void DisplayNextPage()
    {
        currentPage++;
        DisplayPage();
    }

    public void DisplayPreviousPage()
    {
        currentPage--;
        DisplayPage();
    }

    void OnEnable()
    {
        DisplayPage();
    }

    // update change ratio
}
