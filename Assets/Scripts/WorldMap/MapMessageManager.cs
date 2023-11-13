using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMessageManager : MonoBehaviour
{
    public GameObject shader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLevelMessage(int code)
    {   
        Transform auxTrans = transform.Find("LevelPresentationMessage" + code);
        if (auxTrans == null)
            return;
        auxTrans.gameObject.SetActive(true);
        shader.SetActive(true);
    }

    public void ExitMessage(GameObject message)
    {
        shader.SetActive(false);
        message.SetActive(false);
        //isPaused = false;
        //messageOnScreen = false;
    }
}
