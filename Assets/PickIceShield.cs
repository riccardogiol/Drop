using UnityEngine;

public class PickIceShield : MonoBehaviour
{
    public GameObject takeIceShieldBurstPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerShield>().Activate();
                FindObjectOfType<AudioManager>().Play("PickWater");
                PlayIceBurst();
                DestroyIceShield();
                break;
            case "Enemy":
                DestroyIceShield();
                break;
            case "Flame":
                // taggarla come waterdrop? i fuochi dovrebbero cercare di evitarla
                DestroyIceShield();
                break;
            case "FireBullet":
                DestroyIceShield();
                break;
            case "FireWave":
                DestroyIceShield();
                break;
            case "Waterdrop":
                other.GetComponent<PickWaterdrop>().DestroyWaterdrop();
                break;
            case "Superdrop":
                other.GetComponent<PickSuperdrop>().DestroySuperdrop();
                break;
            case "Wall":
                DestroyIceShield();
                break;
            case "Decoration":
                DestroyIceShield();
                break;
            case "Iceshielddrop":
                if (other.gameObject.GetInstanceID() > gameObject.GetInstanceID())
                    DestroyIceShield();
                break;
        }
    }

    public void PlayIceBurst()
    {
        Instantiate(takeIceShieldBurstPrefab, transform.position, Quaternion.identity);
    }

    public void DestroyIceShield()
    {
        Destroy(gameObject);
    }
}
