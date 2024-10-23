using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    protected GameObject panel;

    public void SetUIActive(bool open)
    {
        panel.SetActive(open);
    }
}
