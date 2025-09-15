using UnityEngine;

public class ScaleCircleColliderOverTime : MonoBehaviour
{
    CircleCollider2D circleCollider2D;
    public float timer = 0.7f;
    float countdown;
    public float startingRadius = 1.5f;
    float finalRadius;

    float radiusRange;

    bool isScaling = false;

    void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        finalRadius = circleCollider2D.radius;
        circleCollider2D.radius = startingRadius;
        radiusRange = finalRadius - startingRadius;
        countdown = timer;
    }

    void Start()
    {
        isScaling = true;  
    }

    void Update()
    {
        if (!isScaling)
            return;
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            circleCollider2D.radius = finalRadius;
            isScaling = false;
            return;
        }

        circleCollider2D.radius = startingRadius + radiusRange * ((timer - countdown) / timer);
        
    }
}
