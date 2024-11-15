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
        EventsManager.DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
    }

    private void OnDisable()
    {
        EventsManager.DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
    }


    private void HandleDialogueStarted(DSDialogueSO dialogue)
    {
        switch(dialogue.DialogueType)
        {
            case DSDialogueType.SingleChoice:
                
                break;
            case DSDialogueType.MultipleChoice:
                optionsDialogUI.SetUp(dialogue);
                optionsDialogUI.SetUIActive(true);
                break;
            default:
                // No dialogue (exited out of dialogue?)
                optionsDialogUI.SetUIActive(false);
                break;
        }
    }
}

public class DialogHistory : MonoBehaviour
{
    public static DialogHistory Instance { get; private set; }

    private HashSet<DSDialogueSO> visited;

    private void Awake()
    {
        // Ensure that only one instance exists
        if (Instance == null)
        {
            Instance = this;
            visited = new HashSet<DSDialogueSO>();

            // Optional: keep this instance persistent across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        EventsManager.DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
    }

    private void OnDisable()
    {
        EventsManager.DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
    }

    private void HandleDialogueStarted(DSDialogueSO dialogue)
    {
        visited.Add(dialogue);
    }

    // Optional method to check if a dialogue has been visited
    public bool HasVisited(DSDialogueSO dialogue)
    {
        return visited.Contains(dialogue);
    }
}
