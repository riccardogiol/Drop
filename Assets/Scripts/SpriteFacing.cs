using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFacing : MonoBehaviour
{
    public SpriteRenderer front;
    public SpriteRenderer back;
    public SpriteRenderer left;
    public SpriteRenderer right;

    Animator playerAnimator;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void changeSide(Vector3 facing)
    {
        front.enabled = false;
        back.enabled = false;
        left.enabled = false;
        right.enabled = false;
        if (facing.y < -0.1)
            front.enabled = true;
        else if (facing.y > 0.1)
            back.enabled = true;
        else if (facing.x < -0.1)
            left.enabled = true;
        else
            right.enabled = true;
    }
}
