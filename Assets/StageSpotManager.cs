using System.Collections.Generic;
using UnityEngine;

public class StageSpotManager : MonoBehaviour
{
    List<SpriteRenderer> stageSpots;

    void Awake()
    {
        stageSpots = new List<SpriteRenderer>();
        foreach(Transform child in transform)
        {
            SpriteRenderer auxSR = child.GetComponent<SpriteRenderer>();
            if (auxSR != null)
                stageSpots.Add(auxSR);
        }
    }

    public void ColorStageSpots(int stagesCompleted)
    {
        int i = 0;
        foreach(var spot in stageSpots)
        {
            if (i < stagesCompleted)
            {
                spot.color = new Color(104.0f/255, 189.0f/255, 225.0f/255);
            } else {
                spot.color = new Color(241.0f/255, 154.0f/255, 40.0f/255);
            }
            i++;
        }
    }

}
