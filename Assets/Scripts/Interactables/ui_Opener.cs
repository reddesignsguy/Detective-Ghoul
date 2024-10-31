using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIOpener : Interactee
{
    public GameObject panel;

    public override void Interact()
    {
        Debug.Log("Helol");
        panel.SetActive(true);
    }
}
