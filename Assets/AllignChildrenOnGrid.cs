using System;
using UnityEngine;

public class AllignChildrenOnGrid : MonoBehaviour
{
    void Awake()
    {
        foreach(Transform child in transform)
            child.position = new Vector2((float)Math.Floor(child.position.x) + 0.5f, (float)Math.Floor(child.position.y) + 0.5f);
    }
}
