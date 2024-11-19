using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    public GameObject panel;

    public virtual void SetUIActive(bool open)
    {
        print("Opening: " + panel + "?: " + open);
        if (panel != null)
            panel.SetActive(open);
    }
}
