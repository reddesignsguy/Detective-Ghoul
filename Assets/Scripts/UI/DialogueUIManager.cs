using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DS.ScriptableObjects;
using System;
using System.Collections;

public class DialogueUIManager : UIManager
{
    private DSDialogueSO dialogue;
    public Text dialogueText;
    public float fillOutRate = 14.0f;
    private Coroutine fillOutcoroutine;

    [Obsolete]
    public void StartDialogueUI(Dialogue dialogue)
    {
        SetUIActive(true);  
        UpdateUI(dialogue);
    }

    public void SetUp(DSDialogueSO dialogue)
    {
        this.dialogue = dialogue;
        dialogueText.text = "";
        StopFillingOut();
        fillOutcoroutine = StartCoroutine(FillOutTextCoroutine());

        EventsManager.instance.ShowControls(new Controls { });
    }

    private IEnumerator FillOutTextCoroutine()
    {
        IEnumerator it = dialogue.Text.GetEnumerator();
        while (it.MoveNext())
        {
            dialogueText.text += it.Current;

            // Yield until the next frame
            yield return new WaitForSecondsRealtime(1f/fillOutRate);
        }
        fillOutcoroutine = null;
    }

    [Obsolete]
    public void UpdateUI(Dialogue dialogue)
    {
        dialogueText.text = dialogue.DialogueText;  
    }


    private void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            if (!FillingOutFinished())
            {
                StopFillingOut();
                dialogueText.text = dialogue.Text;
            }
            else
            {
                Skip();
            }
        }
    }

    private bool FillingOutFinished()
    {
        return dialogueText.text == dialogue.Text;
    }

    private void Skip()
    {
        DialogueEvents.instance.StartDialogue(dialogue.Choices[0].NextDialogue);

    }

    public void FinishDialogue(){
         SetUIActive(false);  
    }

    private void StopFillingOut()
    {
        if (fillOutcoroutine != null)
            StopCoroutine(fillOutcoroutine);
        fillOutcoroutine = null;
    }
}
