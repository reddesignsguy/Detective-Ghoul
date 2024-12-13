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

    public Camera cameraTutorial1;
    public Camera cameraTutorial2;

    public AudioSource tenseMusic;
    public AudioSource rain;


    public DialogueTrigger dialogueTrigger;
    public DialogueTrigger dialogueTrigger2;

    public GameObject inventoryBag;
    public GameObject detectiveBook;
    public PlayerMouvement player;
    public IntercablesDetect detect;

    public GameObject girlSprite;
    public GameObject girlChair;

    public GameObject boyPhotograph;
    public GameObject carKey;
    public GameObject doorKey;

    public DSDialogueSO startingScene;

    public Vector3 sittingSpawn = new Vector3(-18.5f, -13.69f, 48.38f);
    public Vector3 standingSpawn = new Vector3(-18.85f, -13.4f, 41.29f);

    public Vector3 sittingTutorial_ChairPosition = new Vector3(-13.0360003f, 0.699999988f, -46.5460014f);
    private Quaternion sittingTutorial_ChairRotation = new Quaternion(0, 0.999537826f, 0, 0.0303991847f);

    public Vector3 standingTutorial_ChairPosition = new Vector3(20.34f,0.69f,-18.75f);
    public Quaternion standingTutorial_ChairRotation = new Quaternion(0, 0.6f, 0, 0.79f);


    public Vector3 girlSittingSpawn = new Vector3(-18.8799992f, -13.8900003f, 38.3699989f);
    public Vector3 girlWaiting = new Vector3(-12.5100002f, -13.5699997f, 41.0200005f);

    public DSDialogueSO firstDialog;
    public DSDialogueSO questionsDialog;
    public InventoryItem firstInspectable;

    // Start is called before the first frame update
    void Start()
    {
        rain.Play();
        inventoryBag.SetActive(false);
        detectiveBook.SetActive(false);
        detect.enabled = false;
        player.PlayAnimation("Standing");
        SetAsMainCamera(cameraTutorial1);
        EventsManager.instance.SetMovement(false);
        DialogueEvents.instance.StartDialogue(firstDialog);

        state = TutorialState.Intro;
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

    void SetupSittingTutorial()
    {
        SetAsMainCamera(cameraTutorial2);
        detect.enabled = true;
        girlChair.transform.SetLocalPositionAndRotation(sittingTutorial_ChairPosition, sittingTutorial_ChairRotation);
        player.transform.position = sittingSpawn;
        girlSprite.transform.position = girlSittingSpawn;
        player.PlayAnimation("Sitting");
        if (girlSprite.TryGetComponent(out Animator animator))
        {
            animator.Play("GirlSitting");
        }
        state = TutorialState.Sitting;

    }

    void SetupStandingTutorial()
    {
        detectiveBook.SetActive(true);
        SetAsMainCamera(player.GetComponentInChildren<Camera>());
        boyPhotograph.SetActive(true);
        girlChair.transform.SetLocalPositionAndRotation(standingTutorial_ChairPosition, standingTutorial_ChairRotation);
        girlSprite.transform.position = girlWaiting ;
        if (girlSprite.TryGetComponent(out Animator animator))
        {
            animator.Play("GirlStanding");
        }

        player.PlayAnimation("Idle");
        player.transform.position = standingSpawn;
        state = TutorialState.Standing;

    }

    void SetupFree()
    {
        state = TutorialState.Finished;
        detectiveBook.SetActive(true);
        EventsManager.instance.FinishTutorial();
    }

    void SetupInventoryInvoker()
    {
        inventoryBag.SetActive(true);
    }


    private void CheckForFirstInspection(InventoryItem item)
    {
        if (item == firstInspectable && state == TutorialState.Standing)
        {
            SetupFree();
        }
    }

    private void CheckForFirstItemToGoInInventory(Interactee interactable)
    {
        if (interactable.gameObject == carKey || interactable.gameObject == doorKey)
        {
            SetupInventoryInvoker();
        }
    }

    private void CheckForFirstDialogueFinished(DSDialogueSO sO)
    {
        if (sO == startingScene && state == TutorialState.Intro)
        {
            SetupSittingTutorial();
        }
    }

    private void CheckForImportantQuestionAsked()
    {
        List<DSDialogueChoiceData> dialogues = questionsDialog.Choices;
        DSDialogueChoiceData choice = dialogues[2];
        bool tutorialQuestionsAnswered = DialogHistory.Instance.HasVisited(choice.NextDialogue);
        if (tutorialQuestionsAnswered)
        {
            SetupStandingTutorial();
        }
    }

    void SetAsMainCamera(Camera newCamera)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Debug.Log("disabling");
            mainCamera.tag = "Untagged"; // Remove the tag from the old main camera
            mainCamera.enabled = false;  // Disable the old main camera
        }

        newCamera.tag = "MainCamera";   // Set new camera as main
        newCamera.enabled = true;       // Enable the new camera
    }

}
