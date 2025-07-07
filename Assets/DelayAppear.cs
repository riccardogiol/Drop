using UnityEngine;

public class DelayAppear : MonoBehaviour
{
    public GameObject subject;
    public float delay = 0f;
    float countdown;

    void Awake()
    {
        subject.SetActive(false);
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            subject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
