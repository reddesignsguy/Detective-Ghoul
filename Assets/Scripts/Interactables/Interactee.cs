using UnityEngine;
using System.Collections;

public abstract class Interactee : MonoBehaviour, Interactable
{
    public string suggestion;

    public string GetSuggestion()
    {
        return suggestion;
    }

    public virtual void Interact()
    {
        return;
    }
}

