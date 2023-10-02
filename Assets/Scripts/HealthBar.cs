using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text hp;

    public GameObject barBackgroundImage;
    public GameObject barMaskImage;
    public GameObject barBackgroundParent;
    public GameObject barMaskParent;
    public GameObject barTop;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        int numIcons = maxHealth/2;
        rectTransform.sizeDelta = new Vector2(numIcons*50, 100);

        GameObject goRef;
        for (int i = 0; i < numIcons; i++)
        {
            goRef = Instantiate(barBackgroundImage, new Vector2(0, 0), Quaternion.identity);
            goRef.transform.SetParent(barBackgroundParent.transform);
            goRef.GetComponent<RectTransform>().anchoredPosition = new Vector2(50*i, 0);
            goRef.transform.localScale = Vector3.one;
    
            goRef = Instantiate(barMaskImage, new Vector2(0, 0), Quaternion.identity);
            goRef.transform.SetParent(barMaskParent.transform);
            goRef.GetComponent<RectTransform>().anchoredPosition = new Vector2(50*i, 0);
            goRef.transform.localScale = Vector3.one;
        }
        barTop.GetComponent<RectTransform>().anchoredPosition = new Vector2(50*numIcons, 0);

    }

    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
        hp.text = currentHealth.ToString();
    }
}
