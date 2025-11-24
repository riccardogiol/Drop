using UnityEngine;

public class SparklerHeroRecharge : MonoBehaviour
{
    public float timer = 1;
    float countdown = 0.5f;
    bool touchingPlayer = false;

    PlayerHealth pHRef;

    void Start()
    {
        if (PlayerPrefs.GetInt("EasyMode", 0) == 1)
            timer = timer * 0.7f;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            countdown = timer;
            if (touchingPlayer && pHRef != null)
                pHRef.FillReservoir(1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayer = true;
            if (pHRef == null)
                pHRef = other.GetComponent<PlayerHealth>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            touchingPlayer = false;
    }


}
