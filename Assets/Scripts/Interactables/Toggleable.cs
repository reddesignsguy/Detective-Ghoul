using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggleable : Interactee
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

    public override void Interact()
    {
        Toggle();
        dialogueTrigger?.TriggerDialogue();
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

}
