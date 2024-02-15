using UnityEngine;

public class LeavesGFXManager : MonoBehaviour
{
    public SpriteRenderer leafRenderer;
    public Sprite[] leafSprites;

    bool isFlowering = true;
    
    public GameObject petalBurstPrefab;

    void Start()
    {
        if (Random.value > 0.5f)
            leafRenderer.flipX = true;
        int index = Random.Range(0, leafSprites.Length);

        leafRenderer.sprite = leafSprites[index];
    }

    public void Plant()
    {
        if (isFlowering)
            return;
        
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