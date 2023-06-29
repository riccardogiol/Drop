using System.Collections;
using UnityEngine;

public class Sparkler : MonoBehaviour
{
    public GameObject prefab;
    public GameObject parent;

    public float timer = 5;

    public bool top;
    public bool down;
    public bool left;
    public bool right;

    void Start () {
		StartCoroutine(SpawnPrefabs());
	}

	IEnumerator SpawnPrefabs() {
		while (true) {
            // add sound and animation
            if (top)
            {
                SpawnPrefab(new Vector3(0, 1, 0));
            }
            if (down)
            {
                SpawnPrefab(new Vector3(0, -1, 0));
            }
            if (left)
            {
                SpawnPrefab(new Vector3(-1, 0, 0));
            }
            if (right)
            {
                SpawnPrefab(new Vector3(1, 0, 0));
            }
			
			yield return new WaitForSeconds(timer);
		}
	}

    void SpawnPrefab(Vector3 movement)
    {
        GameObject goRef = Instantiate(prefab, transform.position + movement/4, Quaternion.identity);
        goRef.transform.parent = parent.transform;
        LinearMovement compRef = goRef.GetComponent<LinearMovement>();
        compRef.enabled = true;
        compRef.startingScale = new Vector3(0, 0, 0);
        compRef.MoveTo(transform.position + movement, 0.5f);
    }
}
