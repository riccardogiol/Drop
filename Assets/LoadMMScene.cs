using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMMScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
