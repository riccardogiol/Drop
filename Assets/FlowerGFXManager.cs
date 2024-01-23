using UnityEngine;

public class FlowerGFXManager : MonoBehaviour
{
    public SpriteRenderer petalRenderer;
    public SpriteRenderer leafRenderer;

    public Sprite[] petalListLvl1;
    public Sprite[] leafListLvl1;


    public Sprite[] petalListLvl2;
    public Sprite[] leafListLvl2;

    public Sprite[] petalListLvl3;
    public Sprite[] leafListLvl3;

    public Color[] colors;

    int indexLvl1 = 0;
    int indexLvl2 = 0;
    int indexLvl3 = 0;

    int currentLevel = 1;
    int maxLevel = 1;
    float timer = 0f;
    readonly float interval = 5f;

    void Start()
    {
        indexLvl1 = Random.Range(0, petalListLvl1.Length);
        indexLvl2 = Random.Range(0, petalListLvl2.Length);
        indexLvl3 = Random.Range(0, petalListLvl3.Length);

        petalRenderer.color = colors[Random.Range(0, colors.Length)];
        if (Random.value > 0.5f)
        {
            petalRenderer.flipX = true;
            leafRenderer.flipX = true;
        }
        maxLevel = Random.Range(1, 4);
        timer = interval;
    }

    void Update()
    {
        if (currentLevel >= maxLevel)
            return;
        if (timer < 0)
        {
            currentLevel++;
            SetGraphic();
            timer = interval;
        }
        timer -= Time.deltaTime;        
    }

    void SetGraphic()
    {
        switch (currentLevel)
        {
            case 1:
                petalRenderer.sprite = petalListLvl1[indexLvl1];
                leafRenderer.sprite = leafListLvl1[indexLvl1];
                break;
            case 2:
                petalRenderer.sprite = petalListLvl2[indexLvl2];
                leafRenderer.sprite = leafListLvl2[indexLvl2];
                break;
            case 3:
                petalRenderer.sprite = petalListLvl3[indexLvl3];
                leafRenderer.sprite = leafListLvl3[indexLvl3];
                break;
        }
    }

    public void Plant()
    {
        currentLevel = 1;

        SetGraphic();

        petalRenderer.enabled = true;
        leafRenderer.enabled = true;

    }

    public void Uproot()
    {
        petalRenderer.enabled = false;
        leafRenderer.enabled = false;
    }

}
