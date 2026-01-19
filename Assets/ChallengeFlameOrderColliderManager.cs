using UnityEngine;

public class ChallengeFlameOrderColliderManager : MonoBehaviour
{
    int index = 0;
    ChallengeFlameOrder challengeFlameOrder;

    public void InitializeValue(ChallengeFlameOrder cfo, int i)
    {
        challengeFlameOrder = cfo;
        index = i;
    }

    void Start()
    {
        if (challengeFlameOrder == null)
             gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (challengeFlameOrder == null) return;
        challengeFlameOrder.ColliderTriggered(index);
    }
}
