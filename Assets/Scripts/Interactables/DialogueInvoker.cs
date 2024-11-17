using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;

public class DialogueInvoker : Interactee
{
    public DSDialogueSO dialogue;

    public override void Interact()
    {
        DialogueEvents.instance.StartDialogue(dialogue);
        print("Interacting w/ NPC");
    }
}
