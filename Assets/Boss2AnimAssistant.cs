using UnityEngine;

public class Boss2AnimAssistant : MonoBehaviour
{
    public SparklerWave sparklerWave;
    public void TriggerWave()
    {
        sparklerWave.TriggerWave(true);
    }
}
