using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    public GameObject panel;

    public void SetUIActive(bool open)
    {
        if (panel != null)
            panel.SetActive(open);
    }

    public virtual void Update() { return;  }
}
