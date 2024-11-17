using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DS.ScriptableObjects;
using System;

public class DialogueUIManager : UIManager
{
    private DSDialogueSO dialogue;
    public Text dialogueText;              

    public void StartDialogueUI(Dialogue dialogue)
    {
        SetUIActive(true);  
        UpdateUI(dialogue);
    }

    public void SetUp(DSDialogueSO dialogue)
    {
        this.dialogue = dialogue;
        dialogueText.text = dialogue.Text;
    }

    public void UpdateUI(Dialogue dialogue)
    {
        dialogueText.text = dialogue.DialogueText;  
    }


    private void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            Skip();
        }
    }

    private void Skip()
    {
        print(dialogue.Choices[0].NextDialogue);
        DialogueEvents.instance.StartDialogue(dialogue.Choices[0].NextDialogue);

    }

    public void FinishDialogue(){
         SetUIActive(false);  
    }
}
