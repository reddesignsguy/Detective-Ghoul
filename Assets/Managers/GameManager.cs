using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using System;
using DS.Data;

public class GameManager : MonoBehaviour
{

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

    public DSDialogueSO startingScene;

    public enum GameState
    {
        SittingTutorial,
        SittingTutorial2,
        StandingTutorial,
        Free
    }

    private GameState state;


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

    // Start is called before the first frame update
    void Start()
    {

        SetAsMainCamera(cameraTutorial1);
        rain.Play();
        //dialogueTrigger.TriggerDialogue();
        DialogueEvents.instance.StartDialogue(firstDialog);
        inventoryBag.SetActive(false);
        detectiveBook.SetActive(false);
        EventsManager.instance.SetMovement(false);
        detect.enabled = false;
        player.PlayAnimation("Standing");
        state = GameState.SittingTutorial;

        // todo: migrate to new dialogue system
    }

    void SetupSittingTutorial2()
    {
        SetAsMainCamera(cameraTutorial2);
        detect.enabled = true;
        girlChair.transform.SetLocalPositionAndRotation(sittingTutorial_ChairPosition, sittingTutorial_ChairRotation);
        //dialogueTrigger2.TriggerDialogue();
        player.transform.position = sittingSpawn;
        girlSprite.transform.position = girlSittingSpawn;
        player.PlayAnimation("Sitting");
        if (girlSprite.TryGetComponent(out Animator animator))
        {
            animator.Play("GirlSitting");
        }
        state = GameState.SittingTutorial2;
    }

    void SetupStandingTutorial()
    {
        detectiveBook.SetActive(true);

        //rain.Play();
        //tenseMusic.Stop();
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
        state = GameState.StandingTutorial;
    }

    void SetupFree()
    {
        detectiveBook.SetActive(true);

        state = GameState.StandingTutorial;
        EventsManager.instance.SetMovement(true);
    }

    void SetupInventoryInvoker()
    {
        inventoryBag.SetActive(true);
    }

    private void OnEnable()
    {
        EventsManager.instance.onImportantInteraction += HandleImportantInteraction;
        EventsManager.instance.onImportantDialogue += HandleImportantDialogue;
        DialogueEvents.instance.onDialogueFinished += HandleDialogueFinished;
        DialogueEvents.instance.onExitedOptions += HandleDialogueUIClosed;
    }


    private void OnDisable()
    {
        EventsManager.instance.onImportantInteraction -= HandleImportantInteraction;
        EventsManager.instance.onImportantDialogue -= HandleImportantDialogue;
        DialogueEvents.instance.onDialogueFinished -= HandleDialogueFinished;
        DialogueEvents.instance.onExitedOptions -= HandleDialogueUIClosed;
    }

    void HandleImportantInteraction(Interactee interactable)
    {
        if (interactable.gameObject == boyPhotograph && state == GameState.StandingTutorial)
        {
            SetupFree();
        }
        else if (interactable.gameObject == carKey)
        {
            SetupInventoryInvoker();
        }
    }

    void HandleImportantDialogue(DialogueTrigger trigger)
    {
        if (trigger == dialogueTrigger && state == GameState.SittingTutorial)
        {
            // sitting tutorial 2
            SetupSittingTutorial2();
        } else if (trigger == dialogueTrigger2 && state == GameState.SittingTutorial2)
        {
            SetupStandingTutorial();
        }
    }

    private void HandleDialogueFinished(DSDialogueSO sO)
    {
        if (sO == startingScene && state == GameState.SittingTutorial)
        {
            SetupSittingTutorial2();
        }

    }

    private void HandleDialogueUIClosed()
    {
        if (state == GameState.SittingTutorial2)
        {
            List<DSDialogueChoiceData> dialogues = questionsDialog.Choices;
            DSDialogueChoiceData choice = dialogues[2];
            bool tutorialQuestionsAnswered = DialogHistory.Instance.HasVisited(choice.NextDialogue);
            if (tutorialQuestionsAnswered)
            {
                SetupStandingTutorial();
            }
        }
    }


    private void Update()
    {
        // Debugging scene progression
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                switch (state)
                {
                    case GameState.SittingTutorial:
                        SetupStandingTutorial();
                        break;
                }
            }
        
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
