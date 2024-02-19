using UnityEngine;

public class LeavesGFXManager : MonoBehaviour
{
    public SpriteRenderer leafRenderer;
    public Sprite[] leafSprites;

    bool isVisible = false;
    bool isFlowering = true;
    
    public GameObject petalBurstPrefab;

    void Awake()
    {
        if (Random.value < 0.6f)
            isVisible = true;
    }

    void Start()
    {
        if (!isVisible)
        {
            leafRenderer.enabled = false;
            return;
        }
        if (Random.value > 0.5f)
            leafRenderer.flipX = true;
        int index = Random.Range(0, leafSprites.Length);
        leafRenderer.sprite = leafSprites[index];
    }

    public void Plant()
    {
        if (isFlowering)
            return;
        if (isVisible)
            leafRenderer.enabled = true;
        isFlowering = true;

        Instantiate(petalBurstPrefab, transform.position, Quaternion.identity);
    }

    public void Uproot()
    {
        leafRenderer.enabled = false;
        isFlowering = false;
    }

}