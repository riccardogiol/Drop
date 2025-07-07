using UnityEngine;

public class FlowerGFXManager : MonoBehaviour
{
    public SpriteRenderer petalRenderer;
    public SpriteRenderer leafRenderer;

    Sprite petalLvl1;
    Sprite leafLvl1;
    Sprite petalLvl2;
    Sprite leafLvl2;
    Sprite petalLvl3;
    Sprite leafLvl3;

    int currentLevel = 1;
    int maxLevel = 1;
    float timer = 0f;
    readonly float interval = 3f;
    bool isFlowering = false;
    bool isVisible = false;

    public GameObject petalBurstPrefab;

    void Awake()
    {
        if (Random.value < 0.7)
            isVisible = true;
        if (Random.value > 0.5f)
        {
            petalRenderer.flipX = true;
            leafRenderer.flipX = true;
        }
        maxLevel = Random.Range(1, 4);
        timer = interval;
    }

    public void SetFlowerGFX(FlowerGFXData fgd)
    {
        petalLvl1 = fgd.petalLvl1;
        leafLvl1 = fgd.leafLvl1;
        petalLvl2 = fgd.petalLvl2;
        leafLvl2 = fgd.leafLvl2;
        petalLvl3 = fgd.petalLvl3;
        leafLvl3 = fgd.leafLvl3;
        petalRenderer.color = fgd.color;
    }

    void Update()
    {
        if (!isVisible)
            return;
        if (!isFlowering)
            return;
        if (currentLevel >= maxLevel)
            return;
        if (timer < 0)
        {
            currentLevel++;
            SetGraphic();
            timer = interval;

            GameObject go = Instantiate(petalBurstPrefab, transform.position, Quaternion.identity);
            go.GetComponent<ParticleSystem>().startColor = petalRenderer.color;
        }
        timer -= Time.deltaTime;
    }

    void SetGraphic()
    {
        switch (currentLevel)
        {
            case 1:
                petalRenderer.sprite = petalLvl1;
                leafRenderer.sprite = leafLvl1;
                break;
            case 2:
                petalRenderer.sprite = petalLvl2;
                leafRenderer.sprite = leafLvl2;
                break;
            case 3:
                petalRenderer.sprite = petalLvl3;
                leafRenderer.sprite = leafLvl3;
                break;
        }
    }

    public void Plant()
    {
        if (isFlowering)
            return;
        currentLevel = 1;

        SetGraphic();
        isFlowering = true;

        if (!isVisible)
            return;

        petalRenderer.enabled = true;
        leafRenderer.enabled = true;

        GameObject go = Instantiate(petalBurstPrefab, transform.position, Quaternion.identity);
        go.GetComponent<ParticleSystem>().startColor = petalRenderer.color;
    }

    public void Uproot()
    {
        petalRenderer.enabled = false;
        leafRenderer.enabled = false;
        isFlowering = false;
    }

    public void setInvisible()
    {
        isVisible = false;
    }

}

public class FlowerGFXData
{
    public Sprite petalLvl1;
    public Sprite leafLvl1;
    public Sprite petalLvl2;
    public Sprite leafLvl2;
    public Sprite petalLvl3;
    public Sprite leafLvl3;
    public Color color;
    public FlowerGFXData(Sprite pl1, Sprite ll1, Sprite pl2, Sprite ll2, Sprite pl3, Sprite ll3, Color c)
    {
        petalLvl1 = pl1;
        leafLvl1 = ll1;
        petalLvl2 = pl2;
        leafLvl2 = ll2;
        petalLvl3 = pl3;
        leafLvl3 = ll3;
        color = c;
    }
}