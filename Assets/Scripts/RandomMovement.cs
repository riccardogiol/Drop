using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public bool completelyRandom = true;
    public float waitingSeconds = 2.0f;
    LinearMovement lm;
    PlaygroundManager playgroundManager;

    List<string> freeDirections;
    string lastPosition;

    public ParticleSystem magicParticles;

    void Start()
    {
        lm = GetComponent<LinearMovement>();
        playgroundManager = FindFirstObjectByType<PlaygroundManager>();

        magicParticles.Play();

        StartCoroutine(NextStep());
    }

    IEnumerator NextStep()
    {
        do
        {
            EvaluateFreeDirections();
            //yield return new WaitForSeconds(2);
            if (freeDirections.Count > 0)
            {
                if (!completelyRandom && freeDirections.Count > 1 && freeDirections.Contains(lastPosition))
                    freeDirections.Remove(lastPosition);
                string randomDirection = freeDirections[Random.Range(0, freeDirections.Count)];
                Vector3 movement;
                switch (randomDirection)
                {
                    case "left":
                        movement = new(-1, 0);
                        lastPosition = "right";
                        break;
                    case "right":
                        movement = new(1, 0);
                        lastPosition = "left";
                        break;
                    case "up":
                        movement = new(0, 1);
                        lastPosition = "down";
                        break;
                    default:
                        movement = new(0, -1);
                        lastPosition = "up";
                        break;
                }
                Vector3 nextCellCenter = playgroundManager.GetCellCenter(transform.position + movement);
                lm.MoveTo(nextCellCenter, System.Math.Min(1f, waitingSeconds*0.7f));
            }
            yield return new WaitForSeconds(Random.Range(waitingSeconds*0.9f, waitingSeconds*1.1f));
        }while(true);
    }

    void EvaluateFreeDirections()
    {
        freeDirections = new List<string>();
        if (FreeTile(transform.position + new Vector3(-1, 0)))
            freeDirections.Add("left"); 
        if (FreeTile(transform.position + new Vector3(1, 0)))
            freeDirections.Add("right"); 
        if (FreeTile(transform.position + new Vector3(0, 1)))
            freeDirections.Add("up"); 
        if (FreeTile(transform.position + new Vector3(0, -1)))
            freeDirections.Add("down");
    }

    bool FreeTile(Vector3 position)
    {
        return !playgroundManager.IsObstacleForFlame(position);
    }
}
