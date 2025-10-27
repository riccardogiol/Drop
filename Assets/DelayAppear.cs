using UnityEngine;

public class DelayAppear : MonoBehaviour
{
    public GameObject subject;
    public float delay = 0f;
    public bool disappear = false;
    float countdown;

    void Awake()
    {
        if (disappear)
            subject.SetActive(true);
        else
            subject.SetActive(false);
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            if (disappear)
            {
                subject.SetActive(false);
                gameObject.SetActive(false);
            } else
            {
                subject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
