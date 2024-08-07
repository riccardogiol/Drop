using UnityEngine;

public class EnemyDirectionController: MonoBehaviour
{
    public Vector2 lastDirection = new(0, -1);
    SpriteFacing spriteFacing;

    void Start()
    {
        spriteFacing = GetComponent<SpriteFacing>();
        if (spriteFacing != null)
            spriteFacing.changeSide(lastDirection);
    }
}
