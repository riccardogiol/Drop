using System;
using UnityEngine;
using UnityEngine.UI;

public class FitBoxText : MonoBehaviour
{
    public Text textComp;
    public RectTransform boxRectTransf;
    public Vector2 padding = new Vector2(40, 20);

    public bool textResize = true;

    void Awake()
    {
        if (textComp == null)
            textComp = GetComponent<Text>();
        if (boxRectTransf == null)
            boxRectTransf = transform.parent.GetComponent<RectTransform>();
    }

    public void Resize()
    {
        if (textComp == null)
            textComp = GetComponent<Text>();
        if (boxRectTransf == null)
            boxRectTransf = transform.parent.GetComponent<RectTransform>();
        
        TextGenerator gen = new TextGenerator();
        Vector2 extents = textComp.rectTransform.rect.size;
        var settings = textComp.GetGenerationSettings(extents);
        float minWidth = gen.GetPreferredWidth(textComp.text, settings) / textComp.pixelsPerUnit;
        float minHeight = gen.GetPreferredHeight(textComp.text, settings) / textComp.pixelsPerUnit;

        if (textResize)
        {
            float ratio = (minWidth + padding.x) / boxRectTransf.sizeDelta.x;
            if (ratio < 1)
                return;
            textComp.fontSize = (int)(textComp.fontSize / ratio);
            return;
        }

        float width = Math.Max(boxRectTransf.sizeDelta.x, minWidth + padding.x);
        float height = Math.Max(boxRectTransf.sizeDelta.y, minHeight + padding.y);

        boxRectTransf.sizeDelta = new Vector2(width, height);
    }
}
