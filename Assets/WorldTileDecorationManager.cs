using System.Collections.Generic;
using UnityEngine;

public class WorldTileDecorationManager : MonoBehaviour
{
    List<ChangeAspect> decorations;

    void Awake()
    {
        decorations = new List<ChangeAspect>();
        foreach(Transform child in transform)
        {
            if (child.GetComponent<ChangeAspect>() != null)
            {
                decorations.Add(child.GetComponent<ChangeAspect>());
                child.GetComponent<ChangeAspect>().SetBurntSprite();
            }
        }
    }

    public void SetGreenValue(float value)
    {   
        int decoToMakeGreen = (int)(value*decorations.Count);
        for (int i = 0; i < decoToMakeGreen; i++)
        {
            decorations[i].SetGreenSprite();
            decorations[i].ColorAdjustment(Random.Range(-0.05f, 0.05f), 0.76f);
        }
    }
}
