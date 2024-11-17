using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using DS.Enumerations;
using System;
using static UnityEngine.Rendering.DebugUI;

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
            DialogueEvents.instance.ExitOptions();
        }
    }

    private void OnEnable()
    {
        DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
    }

    private void OnDisable()
    {
        DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
    }


    private void HandleDialogueStarted(DSDialogueSO dialogue)
    {

        print("Starting dialogue: " + dialogue);

        DSDialogueSO temp = pastDialogue;
        pastDialogue = null;
        DialogueEvents.instance.FinishDialogue(temp);
        pastDialogue = dialogue;

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
                break;
            case DSDialogueType.MultipleChoice:
                optionsDialogUI.SetUp(dialogue);
                optionsDialogUI.SetUIActive(true);
                singleDialogueUI.SetUIActive(false);
                break;
        }

    }
}