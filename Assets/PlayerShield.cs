using UnityEngine;

public class PlayerShield : MonoBehaviour
{

    public float timer = 3f;
    float countdown;

    public bool isActive = false;

    public GameObject shieldGFX;

    // mettere countdown visivo tipo quello delle bombe

    // add some code to make it last loger
    readonly string unlockingCode1 = "ExtraTimeShieldUnlocked";

    void Awake()
    {
        if (PlayerPrefs.GetInt(unlockingCode1, 0) == 1)
            timer += 2;
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer += 1;
    }

    void Update()
    {
        if (isActive)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                isActive = false;
                shieldGFX.SetActive(false);
            }
            //update some graphics like in superdrop?
        }
    }

    public void Activate()
    {
        countdown = timer;
        isActive = true;
        shieldGFX.SetActive(true);
    }
}
