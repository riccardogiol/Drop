using UnityEngine;

public class FireCounter : MonoBehaviour
{
    public float flameValue = 10;
    public float wildfireValue = 30;

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
        return flameCounter * flameValue + wildfireCounter * wildfireValue;
    }
}
