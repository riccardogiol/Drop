using UnityEngine;
using UnityEngine.UI;

public class InitializeButtonSelection : MonoBehaviour
{
    public string buttonName;

    void Start()
    {
        Transform auxTrans = transform.Find(buttonName);
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
    }
}
