using UnityEngine;
using System;

public class BossLifeManager : MonoBehaviour
{
    public Sprite logoOnProgBar;
    public Animator animator;

    public bool secondPhase = false;
    bool inSecondPhase = false;
    public Sprite logoPhase2;

    public GameObject fireBarrier;
    bool barrierUp = false;
    public Collider2D col;

    ProgressionBarFiller progressionBarFiller;
    EnemyHealth enemyHealth;

    PlaygroundManager playgroundManager;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();
        progressionBarFiller = FindFirstObjectByType<ProgressionBarFiller>();
    }

    void Start()
    {
        progressionBarFiller.ShowBossLife();
        progressionBarFiller.SetImage(logoOnProgBar, new Vector3(70, -5, 0), Vector3.one * 0.9f);
        progressionBarFiller.SetMaxValue(1);
        progressionBarFiller.SetMinValue(0);
        progressionBarFiller.SetValue(1);
    }

    public void TakeDamage(int damage)
    {
        int resultingHealth = Math.Max(enemyHealth.currentHealth - damage, 0);
        if (resultingHealth == 0 && secondPhase)
        {
            if (inSecondPhase && damage > 0)
            {
                enemyHealth.currentHealth = 0;
                //some animation?
                return;
            }
            inSecondPhase = true;
            animator.SetTrigger("EnterSecondPhase");
            enemyHealth.currentHealth = 1;

            progressionBarFiller.SetImage(logoPhase2, new Vector3(70, -5, 0), Vector3.one * 0.9f);


            fireBarrier.SetActive(true);
            barrierUp = true;
            col.enabled = false;
        }
        else
        {
            enemyHealth.currentHealth = resultingHealth;
        }
        return;
    }

    public void ScaleLifeBar(float value)
    {
        progressionBarFiller.SetValue(value);
    }

    void Update()
    {
        if (inSecondPhase && barrierUp)
        {
            if (playgroundManager.GetProgressionPerc() >= 0.94)
            {
                fireBarrier.GetComponent<FireBarrierEffectManager>().Estinguish();
                barrierUp = false;
                col.enabled = true;
            }
        }  
    }
}
