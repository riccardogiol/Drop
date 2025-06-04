using System.Collections;
using UnityEngine;

public class ShowAndHide : MonoBehaviour
{
    public bool showDefault = false;
    public string showOnTrue, hideOnTrue;
    void Awake()
    {
        StartCoroutine(OneFrameDelayHideShow());
    }
    
    IEnumerator OneFrameDelayHideShow()
    {
        yield return null;
        if (showDefault)
            {
                if (PlayerPrefs.GetInt(showOnTrue, 0) == 1)
                    yield return null;
                if (PlayerPrefs.GetInt(hideOnTrue, 0) == 1)
                    gameObject.SetActive(false);
            } else {
                if (PlayerPrefs.GetInt(hideOnTrue, 0) == 1)
                    gameObject.SetActive(false);
                if (PlayerPrefs.GetInt(showOnTrue, 0) == 0)
                    gameObject.SetActive(false);
            }
    }

    void Update()
    {

        if (!showDefault && PlayerPrefs.GetInt(hideOnTrue, 0) == 1)
            StartCoroutine(OneFrameDelayDesableObj());
    }
    
    IEnumerator OneFrameDelayDesableObj()
    {
        yield return null;
        gameObject.SetActive(false);
    }

}
