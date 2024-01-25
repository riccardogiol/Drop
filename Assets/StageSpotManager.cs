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
        foreach(var spot in stageSpots)
        {
            if (Random.Range(0, 1.0f) > 0.5f)
                spot.transform.localScale = new Vector3(-1, 1);
            if (i < stagesCompleted)
            {
                foreach (Transform child in spot.transform)
                {
                    if (child.name == "PetalGFX")
                    {
                        child.GetComponent<SpriteRenderer>().sprite = petalList[Random.Range(0, petalList.Length)];
                        child.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
                    }
                    if (child.name == "LeafGFX")
                        child.GetComponent<SpriteRenderer>().sprite = leafList[Random.Range(0, leafList.Length)];
                    if (child.name == "SpotGFX")
                        child.GetComponent<SpriteRenderer>().sprite = greenSpot;
                }
                //spot.color = new Color(104.0f/255, 189.0f/255, 225.0f/255);
            } else {
                foreach (Transform child in spot.transform)
                {
                    if (child.name == "LeafGFX")
                        child.GetComponent<SpriteRenderer>().sprite = burntLeafList[Random.Range(0, burntLeafList.Length)];
                    if (child.name == "SpotGFX")
                        child.GetComponent<SpriteRenderer>().sprite = burntSpot;
                }
                //spot.color = new Color(241.0f/255, 154.0f/255, 40.0f/255);
            }
            i++;
        }
    }

}
