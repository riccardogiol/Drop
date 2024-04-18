using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text hp;

    public GameObject barMaskImage;
    public GameObject barMaskParent;
    public GameObject barTop;
    Animator animator;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = 0;
        rectTransform.sizeDelta = new Vector2(maxHealth*25, 100);

        GameObject goRef;
        for (int i = 0; i < maxHealth; i++)
        {
            goRef = Instantiate(barMaskImage, new Vector2(0, 0), Quaternion.identity);
            goRef.transform.SetParent(barMaskParent.transform);
            goRef.GetComponent<RectTransform>().anchoredPosition = new Vector2(25*i, 0);
            goRef.transform.localScale = Vector3.one;
        }

        barTop.GetComponent<RectTransform>().anchoredPosition = new Vector2(25*maxHealth, 0);
    }

    public void SetHealth(int currentHealth)
    {
        if (currentHealth == 1 && slider.value != 0)
            animator.SetTrigger("LowHealth");
        slider.value = currentHealth;
        hp.text = currentHealth.ToString();
    }
}
