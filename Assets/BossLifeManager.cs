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
        progressionBarFiller.SetImage(logoOnProgBar, Vector3.zero, Vector3.one * 0.31f);
        progressionBarFiller.SetMaxValue(1);
        progressionBarFiller.SetMinValue(0);
        progressionBarFiller.SetValue(1);
    }

    public void TakeDamage(int damage)
    {
        if (barrierUp)
            return;
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

            progressionBarFiller.SetImage(logoPhase2, new Vector3(-63, 10, 0), Vector3.one * 0.9f);


            fireBarrier.SetActive(true);
            barrierUp = true;
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
            }
        }  
    }
}
