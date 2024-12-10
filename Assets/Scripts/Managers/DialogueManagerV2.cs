using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using DS.Enumerations;

public class DialogueManagerV2 : MonoBehaviour
{

    public OptionsDialogUI optionsDialogUI;
    public DialogueUIManager singleDialogueUI;

    public static DialogueManagerV2 Instance;

    private DSDialogueSO pastDialogue;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (pastDialogue != null && pastDialogue.DialogueType == DSDialogueType.MultipleChoice && Input.GetKeyDown(KeyCode.Escape))
        {
            optionsDialogUI.SetUIActive(false);
            pastDialogue = null;
            DialogueEvents.instance.ExitOptions();
        }
    }


    private void OnEnable()
    {
        DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
        EventsManager.instance.onQuestionUIEvent += HandleQuestionUIEvent;

    }

    private void OnDisable()
    {
        DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
        EventsManager.instance.onQuestionUIEvent -= HandleQuestionUIEvent;

    }

    private void HandleQuestionUIEvent(QuestionUIEvent e)
    {
        if (pastDialogue && pastDialogue.DialogueType != DSDialogueType.MultipleChoice)
        {
            return;
        }

        switch (e)
        {
            case QuestionUIEvent.Entered:
                DialogueEvents.instance.SingleDialogueFocused();
                break;
            case QuestionUIEvent.Exited:
                DialogueEvents.instance.MultipleChoiceFocused();
                break;
        }
    }


    private void HandleDialogueStarted(DSDialogueSO dialogue)
    {
        UpdatePastDialogue(dialogue);

        if (dialogue == null)
        {
            optionsDialogUI.SetUIActive(false);
            singleDialogueUI.SetUIActive(false);

            return;
        }

        switch (dialogue.DialogueType)
        {
            case DSDialogueType.SingleChoice:
                singleDialogueUI.SetUp(dialogue);
                singleDialogueUI.SetUIActive(true);
                DialogueEvents.instance.SingleDialogueFocused();
                break;
            case DSDialogueType.MultipleChoice:
                optionsDialogUI.SetUIActive(false);
                optionsDialogUI.SetUp(dialogue);
                optionsDialogUI.SetUIActive(true);

                singleDialogueUI.SetUIActive(false);
                DialogueEvents.instance.MultipleChoiceFocused();
                break;
        }
    }

    private void UpdatePastDialogue(DSDialogueSO dialogue)
    {
        DSDialogueSO temp = pastDialogue;
        pastDialogue = null;
        DialogueEvents.instance.FinishDialogue(temp);
        pastDialogue = dialogue;
    }
}