using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIOpener : Interactee
{
    public UIManager manager;

    public override void Interact()
    {
        manager.SetUIActive(true);
    }
}
