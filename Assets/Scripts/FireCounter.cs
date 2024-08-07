using System;
using UnityEngine;

public class FireCounter : MonoBehaviour
{
    float flameValue = 4;
    float wildfireValue = 9;

    public int flameCounter = 0;
    public int wildfireCounter = 0;
    
    void Start()
    {
        UpdateFireCounters();
    }

    public void UpdateFireCounters()
    {
        flameCounter = 0;
        wildfireCounter = 0;
        foreach(Transform child in transform)
        {
            if(child.gameObject.CompareTag("Flame"))
                flameCounter++;
            if(child.gameObject.CompareTag("Enemy"))
                wildfireCounter++;
        }
    }

    public float FireValue()
    {
        return Math.Max(flameCounter, 0) * flameValue + Math.Max(wildfireCounter, 0) * wildfireValue;
    }
}
