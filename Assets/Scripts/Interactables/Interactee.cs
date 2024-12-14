using UnityEngine;
using System.Collections;

public abstract class Interactee : MonoBehaviour, Interactable
{
    public bool affectsGameState;
    protected Controls inspectControls;
    public Transform suggestionTransform;
    public string suggestion;
    public bool isHidden = false;

    public virtual string GetSuggestion()
    {
        return suggestion;
    }

    public abstract void Interact();

    public bool IsHidden()
    {
        return isHidden;
    }
}

