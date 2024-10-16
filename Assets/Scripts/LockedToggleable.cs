using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedToggleable : Toggleable
{
    [SerializeField]
    private string code;

    private void OnEnable()
    {
        EventsManager.instance.onUnlockAttempt += OnHandleUnlockAttempt;
    }

    private void OnDisable()
    {
        EventsManager.instance.onUnlockAttempt -= OnHandleUnlockAttempt;
    }

    private void OnHandleUnlockAttempt (string code)
    {
        if (this.code == code)
        {
            Toggle();
        }
    }


    public override void Interact()
    {
        Debug.Log("Starting item selection");
    }

    
}
