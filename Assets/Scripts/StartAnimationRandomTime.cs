using UnityEngine;

public class StartAnimationRandomTime : MonoBehaviour
{
    Animator animator;
    public string stateName = "Base Layer.flameMovement";

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator ==  null)
            return;
        animator.Play(stateName, 0, Random.Range(0f, 1f));
    }
}
