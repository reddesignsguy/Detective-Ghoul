using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using DS.Enumerations;
using System;

public class DialogueManagerV2 : MonoBehaviour
{

    public OptionsDialogUI optionsDialogUI;

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

        print("DialogueManagerV2 received dialogue");
        switch(dialogue.DialogueType)
        {
            case DSDialogueType.SingleChoice:
                print("Single dialogue setup");

                break;
            case DSDialogueType.MultipleChoice:
                print("Multiple dialogue set up");
                optionsDialogUI.SetUp(dialogue);
                optionsDialogUI.SetUIActive(true);
                break;
            default:
                print("No dialogue");
                // No dialogue (exited out of dialogue?)
                optionsDialogUI.SetUIActive(false);
                break;
        }
    }
}