using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    public string discordLink = "https://discord.gg/abcd1234";
    public void OpenLink()
    {
        Application.OpenURL(discordLink);
    }
}
