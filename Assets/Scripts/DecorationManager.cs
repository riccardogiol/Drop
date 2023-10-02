using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    public void SetGreenSprites()
    {
        ChangeAspect auxCA;
        foreach(Transform child in transform)
        {
            auxCA = child.GetComponent<ChangeAspect>();
            if (auxCA != null)
                auxCA.SetGreenSprite();
        }

    }
}
