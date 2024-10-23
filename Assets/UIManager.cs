using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    protected GameObject panel;

    public void SetUIActive(bool open)
    {
        Initialize();
        panel.SetActive(open);
    }

    public abstract void Initialize();
}
