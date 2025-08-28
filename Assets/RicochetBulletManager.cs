using UnityEngine;

public class RicochetBulletManager : MonoBehaviour
{
    public GameObject targetSpot;
    public FireBulletRicochet bulletPrefab;

    public void SetTargetSpot(Transform target)
    {
        targetSpot.transform.parent = target;
        targetSpot.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void SetShooterID(int id)
    {
        bulletPrefab.shootingEnemyID = id;
    }

    public void SetPM(PlaygroundManager pm)
    {
        bulletPrefab.playgroundManager = pm;
    }
}
