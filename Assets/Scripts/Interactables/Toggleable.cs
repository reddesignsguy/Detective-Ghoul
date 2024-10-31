using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggleable : MonoBehaviour, Interactable
{
    protected bool on;
    Animator animator;
    private DialogueTrigger dialogueTrigger ;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        on = false;
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    public virtual void Interact()
    {
        Toggle();
        dialogueTrigger.TriggerDialogue();
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

    public virtual string GetSuggestion()
    {
        return "Toggle";
    }
}
