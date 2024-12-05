using System;
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
            {
                auxGo.GetComponent<LevelEnterTrigger>().ActivateButton(false);
                stageSpots.Add(auxGo);
            }
        }
    }

    public void ColorStageSpots(int stagesCompleted)
    {
        int i = 0;
        bool flip;
        foreach(var spot in stageSpots)
        {
            flip = UnityEngine.Random.Range(0, 1.0f) > 0.5f;
            if (i < stagesCompleted)
            {
                int flowerIndex = UnityEngine.Random.Range(0, petalList.Length);
                foreach (Transform child in spot.transform)
                {
                    if (child.name == "PetalGFX")
                    {
                        child.GetComponent<SpriteRenderer>().sprite = petalList[flowerIndex];
                        child.GetComponent<SpriteRenderer>().color = colors[UnityEngine.Random.Range(0, colors.Length)];
                        child.GetComponent<SpriteRenderer>().flipX = flip;
                    }
                    if (child.name == "LeafGFX")
                    {
                        child.GetComponent<SpriteRenderer>().sprite = leafList[flowerIndex];
                        child.GetComponent<SpriteRenderer>().flipX = flip;
                    }
                    if (child.name == "SpotGFX")
                        child.GetComponent<SpriteRenderer>().sprite = greenSpot;
                    if (child.name == "TrailGFX")
                        child.GetComponent<SpriteRenderer>().color = new Color(157f/255, 214f/255, 66f/255);
                }
            } else {
                foreach (Transform child in spot.transform)
                {
                    if (child.name == "LeafGFX")
                    {                        
                        child.GetComponent<SpriteRenderer>().sprite = burntLeafList[UnityEngine.Random.Range(0, burntLeafList.Length)];
                        child.GetComponent<SpriteRenderer>().flipX = flip;
                    }
                    if (child.name == "SpotGFX")
                    {
                        child.GetComponent<SpriteRenderer>().sprite = burntSpot;
                        if (i > stagesCompleted)
                        {
                            child.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
                            child.GetComponent<SinLoopScaling>().enabled = false;
                        }
                    }
                    if (child.name == "TrailGFX")
                        child.GetComponent<SpriteRenderer>().color = new Color(214f/255, 196f/255, 66f/255);
                }
            }
            i++;
        }
    }

    public void ActivateStageSpots(int stagesCompleted)
    {
        int i = 0;
        foreach(var spot in stageSpots)
        {
            if (i <= stagesCompleted)
                spot.GetComponent<LevelEnterTrigger>().ActivateButton(true);
            i++;
        }
    }

    public void ActivateStageSpot(int spotIndex)
    {
        stageSpots[Math.Min(spotIndex-1, stageSpots.Count - 1)].GetComponent<LevelEnterTrigger>().ActivateCollider(true);
    }

    public Vector3 GetStageSpot(int index)
    {
        if (index <= 0 || stageSpots.Count == 0)
            return new Vector3();
        return stageSpots[Math.Min(index-1, stageSpots.Count - 1)].transform.position;
    }

}
