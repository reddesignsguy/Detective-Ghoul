using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public abstract void Interact();
    public abstract string GetSuggestion();
    public abstract bool IsHidden();
}
