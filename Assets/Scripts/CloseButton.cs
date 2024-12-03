using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public UIManager manager;
    public void onClick()
    {
        manager.SetUIActive(false);
    }
}
