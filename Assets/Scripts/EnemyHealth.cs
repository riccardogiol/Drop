using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public bool scaleGFX = true;
    public Transform enemyGFX;
    public GameObject vaporBurstPrefab;
    public FireCounter flameParent;


    public bool isBoss = false;
    public bool healthOnProgBar = false;
    public float winningSceneTimer = 8.0f;
    public Sprite logoOnProgBar;
    public Animator animator;
    ProgressionBarFiller progressionBarFiller;
    BossLifeManager bossLifeManager;

    ChallengeBoss3 challengeEnvElements;
    ChallengeBoss4 challengeIceBullet;

    void Awake()
    {
        if (isBoss)
            bossLifeManager = GetComponent<BossLifeManager>();
        
        challengeEnvElements = FindFirstObjectByType<ChallengeBoss3>();
        challengeIceBullet = FindFirstObjectByType<ChallengeBoss4>();
    }

    void Start()
    {
        currentHealth = Math.Min(maxHealth, currentHealth);
        if (healthOnProgBar)
        {
            progressionBarFiller = FindFirstObjectByType<ProgressionBarFiller>();
            progressionBarFiller.ShowBossLife();
            progressionBarFiller.SetImage(logoOnProgBar, new Vector3(-65, 20, 0), Vector3.one*0.9f);
            progressionBarFiller.SetMaxValue(1);
            progressionBarFiller.SetMinValue(0);
            progressionBarFiller.SetValue(1);
            flameParent = FindFirstObjectByType<FireCounter>();
        }
        ScaleOnHealth();
    }

    public void ScaleOnHealth()
    {
        if (healthOnProgBar)
            progressionBarFiller.SetValue((float)currentHealth/maxHealth);
        if (isBoss)
            bossLifeManager.ScaleLifeBar((float)currentHealth/maxHealth);
        if (scaleGFX)
            {
                float scale = (float)currentHealth / maxHealth * 0.4f + 0.6f;
                enemyGFX.localScale = new Vector3(scale, scale, 1);
            }
    }

    public void TakeDamage(int damage, bool envDamage = false, bool isIceBullet = false)
    {
        if (currentHealth <= 0)
            return;
        
        if (isBoss)
            bossLifeManager.TakeDamage(damage);
        else
            currentHealth = Math.Max(currentHealth - damage, 0);
        
        ScaleOnHealth();

        if (currentHealth <= 0)
        {
            if (healthOnProgBar || isBoss)
            {
                if (challengeIceBullet != null && isIceBullet)
                    challengeIceBullet.IncreaseCounter();
                if (flameParent != null)
                    flameParent.DestroyAllFires();
                if (animator != null)
                    animator.SetTrigger("Die");
                FindFirstObjectByType<StageManager>().WinGame(true, winningSceneTimer);
            }
            else
            {
                PlaygroundManager pgRef = FindObjectOfType<PlaygroundManager>();
                if (pgRef != null)
                    pgRef.WildfireEstinguished();
                if (challengeEnvElements != null && envDamage)
                    challengeEnvElements.IncreaseCounter();
                if (challengeIceBullet != null && isIceBullet)
                    challengeIceBullet.IncreaseCounter();
                Instantiate(vaporBurstPrefab, transform.position, Quaternion.identity);
                Instantiate(vaporBurstPrefab, transform.position + new Vector3(0, 0.5f), Quaternion.identity);
                Destroy(gameObject);
            }
        } else if (animator != null)
            animator.SetTrigger("Hit");
    }
    

    public void FillReservoir(int value)
    {
        currentHealth = Math.Min( currentHealth + value, maxHealth);
        ScaleOnHealth();
    }
}
