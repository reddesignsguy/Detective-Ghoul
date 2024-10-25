using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogues; 

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogueSequence(dialogues);
    }
}
