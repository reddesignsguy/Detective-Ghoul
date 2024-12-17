using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using System;
using DS.Data;
using static UnityEditor.Progress;

public class TutorialManager : MonoBehaviour
{
    public GameObject carKey;
    public GameObject doorKey;
    public DSDialogueSO startingScene;
    public DSDialogueSO firstDialog;
    public DSDialogueSO questionsDialog;
    public InventoryItem firstInspectable;
    public TutorialSetup tutorialScene;

    public enum TutorialState
    {
        Intro,
        Sitting,
        Standing,
        Finished
    }
    private TutorialState state = TutorialState.Intro;

    void Start()
    {
        SetState(TutorialState.Intro);
    }

    private void OnEnable()
    {
        EventsManager.instance.onImportantInteraction += HandleImportantInteraction;
        EventsManager.instance.onPickupItem += HandlePickedupInventoryItem;
        EventsManager.instance.OnLeftItem += HandleLeftInventoryItem;
        DialogueEvents.instance.onDialogueFinished += HandleDialogueFinished;
        DialogueEvents.instance.onExitedOptions += HandleDialogueUIClosed;
    }

    private void OnDisable()
    {
        EventsManager.instance.onImportantInteraction -= HandleImportantInteraction;
        EventsManager.instance.onPickupItem -= HandlePickedupInventoryItem;
        EventsManager.instance.OnLeftItem -= HandleLeftInventoryItem;
        DialogueEvents.instance.onDialogueFinished -= HandleDialogueFinished;
        DialogueEvents.instance.onExitedOptions -= HandleDialogueUIClosed;
    }

    private void HandleLeftInventoryItem(InventoryItem item)
    {
        CheckForFirstInspection(item);
    }

    private void HandlePickedupInventoryItem(InventoryItem item)
    {
        CheckForFirstInspection(item);
    }

    void HandleImportantInteraction(Interactee interactable)
    {
        CheckForFirstItemToGoInInventory(interactable);
    }

    private void HandleDialogueFinished(DSDialogueSO sO)
    {
        CheckForFirstDialogueFinished(sO);
    }

    private void HandleDialogueUIClosed()
    {
        if (state == TutorialState.Sitting)
        {
            CheckForImportantQuestionAsked();
        }
    }

    private void CheckForFirstInspection(InventoryItem item)
    {
        if (item == firstInspectable && state == TutorialState.Standing)
        {
            SetState(TutorialState.Finished);
        }
    }

    private void CheckForFirstItemToGoInInventory(Interactee interactable)
    {
        if (interactable.gameObject == carKey || interactable.gameObject == doorKey)
        {
            tutorialScene.FirstInspectionOccurred();
        }
    }

    private void CheckForFirstDialogueFinished(DSDialogueSO sO)
    {
        if (sO == startingScene && state == TutorialState.Intro)
        {
            SetState(TutorialState.Sitting);
        }
    }

    private void CheckForImportantQuestionAsked()
    {
        List<DSDialogueChoiceData> dialogues = questionsDialog.Choices;
        DSDialogueChoiceData choice = dialogues[2];
        bool tutorialQuestionsAnswered = DialogHistory.Instance.HasVisited(choice.NextDialogue);
        if (tutorialQuestionsAnswered)
        {
            SetState(TutorialState.Standing);
        }
    }

    private void SetState(TutorialState state)
    {
        switch (state)
        {
            case TutorialState.Intro:
                EventsManager.instance.SetMovement(false);
                DialogueEvents.instance.StartDialogue(firstDialog);
                break;
            case TutorialState.Sitting:
                break;
            case TutorialState.Standing:
                break;
            case TutorialState.Finished:
                EventsManager.instance.FinishTutorial();
                break;
        }

        this.state = state;
        tutorialScene.Setup(state);
    }

}
