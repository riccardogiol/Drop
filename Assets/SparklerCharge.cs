using System;
using UnityEngine;

public class SparklerCharge : MonoBehaviour
{

    public int maxCharge = 10;
    public int currentCharge = 0;

    public bool scaleGFX = true;
    public Transform waterGFX;

    public GameObject waterBurstPrefab;
    public ParticleSystem waterSparklesParticles;

    SparklerWave sparklerWave;

    void Awake()
    {
        sparklerWave = GetComponent<SparklerWave>();
    }

    void Start()
    {
        if (sparklerWave != null && currentCharge < maxCharge)
        {
            sparklerWave.enabled = false;
            waterSparklesParticles.Pause();
        }
        ScaleOnHealth();
    }

    public void ScaleOnHealth()
    {
        if (scaleGFX)
        {
            float scale = (float)currentCharge/maxCharge*0.8f + 0.2f;
            waterGFX.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void FillReservoir(int charge)
    {
        if (currentCharge == maxCharge)
            return;

        currentCharge = Math.Min(currentCharge + charge, maxCharge);
        ScaleOnHealth();
        if (currentCharge == maxCharge)
        {
            Instantiate(waterBurstPrefab, transform.position, Quaternion.identity);
            sparklerWave.enabled = true;
            waterSparklesParticles.Play();
        }
    }

}
