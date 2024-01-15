using UnityEngine;

public class SpriteFacing : MonoBehaviour
{
    public Animator bodyAnimator;
    public Animator faceAnimator;

    public void changeSide(Vector3 facing)
    {
        bodyAnimator.SetFloat("Horizontal", facing.x);
        bodyAnimator.SetFloat("Vertical", facing.y);
    
        faceAnimator.SetFloat("Horizontal", facing.x);
        faceAnimator.SetFloat("Vertical", facing.y);
    }
}
