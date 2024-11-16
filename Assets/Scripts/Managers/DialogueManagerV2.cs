using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using DS.Enumerations;
using System;

public class DialogueManagerV2 : MonoBehaviour
{

    public OptionsDialogUI optionsDialogUI;
    public DialogueUIManager singleDialogueUI;

    public static DialogueManagerV2 Instance;

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
        if (dialogue == null)
        {
            optionsDialogUI.SetUIActive(false);
            singleDialogueUI.SetUIActive(false);
            return;
        }

        switch(dialogue.DialogueType)
        {
            case DSDialogueType.SingleChoice:
                print("Single dialogue setup");
                singleDialogueUI.SetUp(dialogue);
                singleDialogueUI.SetUIActive(true);
                break;
            case DSDialogueType.MultipleChoice:
                print("Multiple dialogue set up");
                optionsDialogUI.SetUp(dialogue);
                optionsDialogUI.SetUIActive(true);
                singleDialogueUI.SetUIActive(false);
                break;
        }
    }
}