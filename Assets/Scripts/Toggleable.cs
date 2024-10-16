using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggleable : MonoBehaviour, Interactable
{
    protected bool on;

    private void Awake()
    {
        on = false;
    }

    public virtual void Interact()
    {
        Toggle();
    }

    protected void Toggle()
    {
        Debug.Log("Toggling object");
    }
}
