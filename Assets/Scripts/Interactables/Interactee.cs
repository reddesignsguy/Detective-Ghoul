using UnityEngine;
using System.Collections;

public abstract class Interactee : MonoBehaviour, Interactable
{
    public Transform suggestionTransform;
    public string suggestion;

    public virtual string GetSuggestion()
    {
        return suggestion;
    }

    public abstract void Interact();
}

