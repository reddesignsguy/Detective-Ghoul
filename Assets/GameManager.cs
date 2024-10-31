using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    public DialogueTrigger dialogueTrigger2;

    public GameObject inventoryBag;
    public PlayerMouvement player;
    public IntercablesDetect detect;

    public GameObject girlSprite;
    public GameObject girlChair;

    public GameObject boyPhotograph;

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
    public Vector3 standingTutorial_ChairPosition = new Vector3(20.34f,0.69f,-18.75f);
    public Quaternion standingTutorial_ChairRotation = new Quaternion(0, 0.6f, 0, 0.79f);


    public Vector3 girlSittingSpawn = new Vector3(-18.8799992f, -13.8900003f, 38.3699989f);
    public Vector3 girlWaiting = new Vector3(-12.5100002f, -13.5699997f, 41.0200005f);

    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger.TriggerDialogue();
        inventoryBag.SetActive(false);;
        EventsManager.instance.SetMovement(false);
        detect.enabled = false;
        player.PlayAnimation("Standing");
        state = GameState.SittingTutorial;
    }

    void SetupSittingTutorial2()
    {
        dialogueTrigger2.TriggerDialogue();
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
        detect.enabled = true;

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
        inventoryBag.SetActive(true);
        state = GameState.StandingTutorial;
        EventsManager.instance.SetMovement(true);
    }

    private void OnEnable()
    {
        EventsManager.instance.onImportantInteraction += HandleImportantInteraction;
        EventsManager.instance.onImportantDialogue += HandleImportantDialogue;
    }


    private void OnDisable()
    {
        EventsManager.instance.onImportantInteraction -= HandleImportantInteraction;
        EventsManager.instance.onImportantDialogue -= HandleImportantDialogue;
    }

    void HandleImportantInteraction(Interactee interactable)
    {
        if (interactable.gameObject == boyPhotograph && state == GameState.StandingTutorial)
        {
            SetupFree();
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
}
