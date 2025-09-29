using UnityEngine;
using UnityEngine.UI;

public class LoadSprite : MonoBehaviour
{
    Image image;
    public string code = "Sprites/Elements/";

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        image.sprite = Resources.Load<Sprite>(code);
    }
}
