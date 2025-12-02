using UnityEngine;
using UnityEngine.UI;

public class InitializeButtonSelection : MonoBehaviour
{
    public string buttonName;

    void OnEnable()
    {
        Transform auxTrans = transform.Find(buttonName);
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
    }

    public void Refresh()
    {
        Transform auxTrans = transform.Find(buttonName);
        if (auxTrans == null)
            return;
        auxTrans.GetComponent<Button>().Select();
    }
}
