using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using System;
using DS.Data;
using static UnityEditor.Progress;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        Intro,
        Sitting,
        Standing,
        Finished
    }
    private TutorialState state = TutorialState.Intro;

    public GameObject carKey;
    public GameObject doorKey;

    public DSDialogueSO startingScene;

    public DSDialogueSO firstDialog;
    public DSDialogueSO questionsDialog;
    public InventoryItem firstInspectable;

    public TutorialSetup tutorialScene;


    //public GameObject boyPhotograph;

    //public Camera cameraTutorial1;
    //public Camera cameraTutorial2;

    //public AudioSource tenseMusic;
    //public AudioSource rain;

    //public GameObject inventoryBag;
    //public GameObject detectiveBook;
    //public PlayerMouvement player;
    //public IntercablesDetect detect;

    //public GameObject girlSprite;
    //public GameObject girlChair;

    //public Vector3 sittingSpawn = new Vector3(-18.5f, -13.69f, 48.38f);
    //public Vector3 standingSpawn = new Vector3(-18.85f, -13.4f, 41.29f);

    //public Vector3 sittingTutorial_ChairPosition = new Vector3(-13.0360003f, 0.699999988f, -46.5460014f);
    //private Quaternion sittingTutorial_ChairRotation = new Quaternion(0, 0.999537826f, 0, 0.0303991847f);

    //public Vector3 standingTutorial_ChairPosition = new Vector3(20.34f, 0.69f, -18.75f);
    //public Quaternion standingTutorial_ChairRotation = new Quaternion(0, 0.6f, 0, 0.79f);


    //public Vector3 girlSittingSpawn = new Vector3(-18.8799992f, -13.8900003f, 38.3699989f);
    //public Vector3 girlWaiting = new Vector3(-12.5100002f, -13.5699997f, 41.0200005f);

    // Start is called before the first frame update
    void Start()
    {
        SetState(TutorialState.Intro);
    }

    private void OnEnable()
    {
        EventsManager.instance.onImportantInteraction += HandleImportantInteraction;
        EventsManager.instance.onPickUpInventoryItem += HandlePickedupInventoryItem;
        EventsManager.instance.onLeftInventoryItem += HandleLeftInventoryItem;
        DialogueEvents.instance.onDialogueFinished += HandleDialogueFinished;
        DialogueEvents.instance.onExitedOptions += HandleDialogueUIClosed;
    }

    private void OnDisable()
    {
        EventsManager.instance.onImportantInteraction -= HandleImportantInteraction;
        EventsManager.instance.onPickUpInventoryItem -= HandlePickedupInventoryItem;
        EventsManager.instance.onLeftInventoryItem -= HandleLeftInventoryItem;
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
