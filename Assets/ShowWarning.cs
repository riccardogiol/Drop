using UnityEngine;

public class ShowWarning : MonoBehaviour
{
    public GameObject warningPanel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerHealth>().currentHealth >= 6)
                warningPanel.SetActive(true);
            Destroy(gameObject);
        }
    }
}
