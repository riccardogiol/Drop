using System;
using System.Collections.Generic;
using UnityEngine;

public class StageSpotManager : MonoBehaviour
{
    public Sprite[] petalList;
    public Sprite[] leafList;
    public Sprite[] burntLeafList;

    public Color[] colors;

    List<GameObject> stageSpots;

    public int lvlCode = 0;
    float delaySin = 0.3f;

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

        for(int i = 0; i < stageSpots.Count - 1; i++)
        {
            GameObject spot = stageSpots[i];
            GameObject nextSpot = stageSpots[i+1];
            Vector3 direction = nextSpot.transform.position - spot.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f;
            foreach(Transform child in spot.transform)
            {
                if (child.name == "TrailGFX")
                {
                    child.SetPositionAndRotation(spot.transform.position + direction/2.0f, Quaternion.Euler(0, 0, angle));
                }
            }

        }
    }

    public void ColorStageSpots(int stagesCompleted)
    {
        SaveData saveData = SaveManager.Load();
        bool medalWon = false;
        int i = 0, stageSaveIdx;
        bool flip;
        foreach(var spot in stageSpots)
        {
            flip = UnityEngine.Random.Range(0, 1.0f) > 0.5f;
            if (i < stagesCompleted)
            {
                if (saveData.StageChallengeRecords != null)
                {
                    stageSaveIdx = (lvlCode - 1) * 4 + (i+1);
                    medalWon = saveData.StageCompleteStatus[stageSaveIdx] == 2; 
                }
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
                    {
                        child.GetComponent<SpriteRenderer>().color = new Color(157f/255, 214f/255, 66f/255);
                        child.GetComponent<SinLoopScaling>().delay = lvlCode*4*delaySin + i*delaySin;
                        if (medalWon)
                        {
                            foreach (Transform grandChild in child.transform)
                            {
                                if (grandChild.name == "MedalGFX")
                                    grandChild.gameObject.SetActive(true);
                            }
                        }
                    }
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
                        child.GetComponent<SpriteRenderer>().color = new Color(214f/255, 196f/255, 66f/255);
                        if (i > stagesCompleted)
                        {
                            child.GetComponent<SpriteRenderer>().color = new Color(214f/320, 196f/320, 66f/320); // darker
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

    public void DisableAllStageSpots()
    {
        foreach(var spot in stageSpots)
        {
            spot.GetComponent<LevelEnterTrigger>().ActivateButton(false);
            foreach (Transform child in spot.transform)
            {
                if (child.name == "LeafGFX")
                {                        
                    child.GetComponent<SpriteRenderer>().sprite = burntLeafList[UnityEngine.Random.Range(0, burntLeafList.Length)];
                    child.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
                }
                if (child.name == "SpotGFX")
                {
                    child.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
                    child.GetComponent<SinLoopScaling>().enabled = false;
                }
                if (child.name == "TrailGFX")
                    child.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
            }
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
