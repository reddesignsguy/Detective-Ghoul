using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public List<Dialogue> Dialogues;  
    private int CurrentIndex = 0;     
    private Dialogue currentDialogue;

    void Start()
    {
        if (Dialogues.Count > 0)
        {
            currentDialogue = Dialogues[CurrentIndex];
            StartDialogue(currentDialogue); 
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        EventsManager.instance.StartDialogue(dialogue); 
    }

    public void OnOptionSelected(int selectedOptionId)
    {
        Option selectedOption = currentDialogue.options.Find(option => option.id == selectedOptionId);

        if (selectedOption != null)
        {
            CurrentIndex = selectedOption.NextDialogueIndex;

            if (CurrentIndex < Dialogues.Count)
            {
                StartDialogue(Dialogues[CurrentIndex]);
            }
            else
            {
                Debug.Log("Dialogue sequence finished.");
            }
        }
    }
}
