using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public GameObject panel;
    public void onClick()
    {
        panel.SetActive(false);
    }
}
