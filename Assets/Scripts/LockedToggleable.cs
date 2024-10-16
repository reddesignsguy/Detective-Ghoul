using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedToggleable : Toggleable
{
    public override void Interact()
    {
        Debug.Log("Starting item selection");
    }
}
