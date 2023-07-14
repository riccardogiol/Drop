using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFacing : MonoBehaviour
{
    public SpriteRenderer front;
    public SpriteRenderer back;
    public SpriteRenderer left;
    public SpriteRenderer right;

    public void changeSide(Vector3 facing)
    {
        front.enabled = false;
        back.enabled = false;
        left.enabled = false;
        right.enabled = false;
        if (facing == new Vector3(0, -1, 0))
            front.enabled = true;
        else if (facing == new Vector3(0, 1, 0))
            back.enabled = true;
        else if (facing == new Vector3(-1, 0, 0))
            left.enabled = true;
        else if (facing == new Vector3(1, 0, 0))
            right.enabled = true;
    }
}
