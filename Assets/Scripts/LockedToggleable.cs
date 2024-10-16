using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedToggleable : Toggleable
{

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    public override void Interact()
    {
        Debug.Log("Starting item selection");
    }

    
}
