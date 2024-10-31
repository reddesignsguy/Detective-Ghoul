using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DialogueTrigger dialogues_Scene1;

    public GameObject inventoryBag;
    public PlayerMouvement player;
    public IntercablesDetect detect;

    public GameObject girlSprite;
    public GameObject girlChair;

    public GameObject boyPhotograph;

    public enum GameState
    {
        SittingTutorial,
        StandingTutorial,
        Free
    }

    private GameState state;

    public Vector3 sittingSpawn = new Vector3(-20.84f, -13.69f, 48.38f);
    public Vector3 standingSpawn = new Vector3(-18.85f, -13.4f, 41.29f);
    public Vector3 standingTutorial_ChairPosition = new Vector3(20.34f,0.69f,-18.75f);
    public Quaternion standingTutorial_ChairRotation = new Quaternion(0, 0.6f, 0, 0.79f);

    // Start is called before the first frame update
    void Start()
    {
        inventoryBag.SetActive(false);
        EventsManager.instance.SetMovement(false);
        player.transform.position = sittingSpawn;
        detect.enabled = false;
        player.PlayAnimation("Sitting");
        state = GameState.SittingTutorial;
    }

    void SetupStandingTutorial()
    {
        detect.enabled = true;
        girlSprite.SetActive(false);
        girlChair.transform.SetLocalPositionAndRotation(standingTutorial_ChairPosition, standingTutorial_ChairRotation);
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
    }

    void HandleImportantInteraction(Interactee interactable)
    {
        if (interactable.gameObject == boyPhotograph && state == GameState.StandingTutorial)
        {
            SetupFree();
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
