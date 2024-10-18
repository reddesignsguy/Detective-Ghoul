using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggleable : MonoBehaviour, Interactable
{
    protected bool on;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        on = false;
    }

    public virtual void Interact()
    {
        Toggle();
    }

    protected void Toggle()
    {
        Debug.Log("Toggling object");

        if (animator)
        {
            bool curAnimationState = animator.GetBool("On");
            animator.SetBool("On", !curAnimationState);
        }
    }

    public string GetSuggestion()
    {
        return "Toggle";
    }
}
