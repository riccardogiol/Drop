using UnityEngine;
using UnityEngine.UI;

public class FlowerBarFiller : MonoBehaviour
{
    float maxExtensionSlider = 100;
    float minExtensionSlider = 5;
    public GameObject mask;
    public Image leafImage;
    public Image petalImage;

    void Start()
    {
        SetValue(minExtensionSlider/maxExtensionSlider);
    }

    public void SetGFX(FlowerGFXData flowerGFXData)
    {
        leafImage.sprite = flowerGFXData.leafLvl3;
        petalImage.sprite = flowerGFXData.petalLvl3;
        petalImage.color = flowerGFXData.color;
    }

    public void SetValue(float value)
    {
        mask.GetComponent<RectTransform>().sizeDelta = new Vector2(100, minExtensionSlider + value * (maxExtensionSlider-minExtensionSlider));
    }
}
