using UnityEngine;
using UnityEngine.UI;

public class ChallengeInfo : MonoBehaviour
{
    Text textComp;

    void Awake()
    {
      textComp = GetComponent<Text>();  
    }
    public void WriteText(string info)
    {
        textComp.text = info;
    }
}
