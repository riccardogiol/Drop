using UnityEngine;
using UnityEngine.UI;

public class ChallengeInfo : MonoBehaviour
{
    public Text textComp;
    public Image medalGFX;

    public void WriteText(string info)
    {
        textComp.text = info;
    }
}
