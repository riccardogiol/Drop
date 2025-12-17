using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelfButtonsManager : MonoBehaviour
{
    RectTransform rectTransform;
    public Button nextButton, previousButton;
    public RectTransform[] elements;
    List<GameObject> extraElements;
    public Text pageNumber;

    float buttonHeight = 80, buttonsDistance = 10, verticalSpace, horizontalSpace;
    int maxElementsToDisplay, currentPage;

    public InitializeButtonSelection ibs;
    public bool squaredButton = false;
    bool onePage = false;

    public GameObject emptyElPrefab;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        verticalSpace = rectTransform.rect.height;
        horizontalSpace = rectTransform.rect.width;
        maxElementsToDisplay = (int)Math.Floor(verticalSpace / (buttonHeight + buttonsDistance));
        if (squaredButton)
            maxElementsToDisplay = 9;
        if (elements.Length <= maxElementsToDisplay)
        {
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            pageNumber.gameObject.SetActive(false);
            onePage = true;
        }
        currentPage = 0;
        DisplayPage();
    }

    void DisplayPage()
    {
        pageNumber.text = "" + (currentPage+1);
        int lastElement = Mathf.Min(maxElementsToDisplay * (currentPage + 1), elements.Length);
        int firstElement = Math.Max(lastElement - maxElementsToDisplay, 0);
        float lastPosition = 0;
        float horizontalStep = horizontalSpace/4.0f;
        float verticalStep = verticalSpace/4.0f;
        float verticalExtra = 25;
        Vector2 startingPositionXY = new Vector2(horizontalStep, verticalSpace - verticalStep + verticalExtra);
        int ed = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            if (i < firstElement)
                elements[i].gameObject.SetActive(false);
            else if (i >= lastElement)
                elements[i].gameObject.SetActive(false);
            else
            {
                if (squaredButton)
                {
                    elements[i].anchoredPosition = startingPositionXY + new Vector2(horizontalStep * (i%3), - (verticalStep + verticalExtra) * (i/3));
                    elements[i].gameObject.SetActive(true);
                    ed ++;
                }
                else 
                {
                    elements[i].anchoredPosition = new Vector2(0, lastPosition);
                    lastPosition -= buttonHeight + buttonsDistance;
                    elements[i].gameObject.SetActive(true);
                }
            }
        }

        if (emptyElPrefab != null && extraElements == null && squaredButton && ed < maxElementsToDisplay)
        {
            extraElements = new List<GameObject>();
            for (int i = ed; i < maxElementsToDisplay; i++)
            {
                GameObject goRef = Instantiate(emptyElPrefab, transform);
                goRef.transform.parent = transform;
                extraElements.Add(goRef);
                goRef.GetComponent<RectTransform>().anchoredPosition = startingPositionXY + new Vector2(horizontalStep * (i%3), - (verticalStep + verticalExtra) * (i/3));
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
