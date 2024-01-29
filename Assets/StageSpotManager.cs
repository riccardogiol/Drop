using System.Collections.Generic;
using UnityEngine;

public class StageSpotManager : MonoBehaviour
{
    
    public Sprite burntSpot;
    public Sprite greenSpot;

    public Sprite[] petalList;
    public Sprite[] leafList;
    public Sprite[] burntLeafList;

    public Color[] colors;

    List<GameObject> stageSpots;

    void Awake()
    {
        stageSpots = new List<GameObject>();
        foreach(Transform child in transform)
        {
            GameObject auxGo = child.gameObject;
            if (auxGo != null)
                stageSpots.Add(auxGo);
        }
    }

    public void ColorStageSpots(int stagesCompleted)
    {
        int i = 0;
        bool flip;
        foreach(var spot in stageSpots)
        {
            flip = Random.Range(0, 1.0f) > 0.5f;
            if (i < stagesCompleted)
            {
                foreach (Transform child in spot.transform)
                {
                    if (child.name == "PetalGFX")
                    {
                        child.GetComponent<SpriteRenderer>().sprite = petalList[Random.Range(0, petalList.Length)];
                        child.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
                        child.GetComponent<SpriteRenderer>().flipX = flip;
                    }
                    if (child.name == "LeafGFX")
                    {
                        child.GetComponent<SpriteRenderer>().sprite = leafList[Random.Range(0, leafList.Length)];
                        child.GetComponent<SpriteRenderer>().flipX = flip;
                    }
                    if (child.name == "SpotGFX")
                        child.GetComponent<SpriteRenderer>().sprite = greenSpot;
                    if (child.name == "TrailGFX")
                        child.GetComponent<SpriteRenderer>().color = new Color(22f/255, 50f/255, 0f/255);
                }
            } else {
                foreach (Transform child in spot.transform)
                {
                    if (child.name == "LeafGFX")
                    {                        
                        child.GetComponent<SpriteRenderer>().sprite = burntLeafList[Random.Range(0, burntLeafList.Length)];
                        child.GetComponent<SpriteRenderer>().flipX = flip;
                    }
                    if (child.name == "SpotGFX")
                        child.GetComponent<SpriteRenderer>().sprite = burntSpot;
                    if (child.name == "TrailGFX")
                        child.GetComponent<SpriteRenderer>().color = new Color(56f/255, 45f/255, 24f/255);
                }
            }
            i++;
        }
    }

}
