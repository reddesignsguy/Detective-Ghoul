using UnityEngine;
using static TutorialManager;

public class TutorialSetup : MonoBehaviour
{
    public GameObject boyPhotograph;

    public Camera cameraTutorial1;
    public Camera cameraTutorial2;

    public AudioSource tenseMusic;
    public AudioSource rain;

    public GameObject inventoryBag;
    public GameObject detectiveBook;
    public PlayerMouvement player;
    public IntercablesDetect detect;

    public GameObject girlSprite;
    public GameObject girlChair;

    public Vector3 sittingSpawn = new Vector3(-18.5f, -13.69f, 48.38f);
    public Vector3 standingSpawn = new Vector3(-18.85f, -13.4f, 41.29f);

    public Vector3 sittingTutorial_ChairPosition = new Vector3(-13.0360003f, 0.699999988f, -46.5460014f);
    private Quaternion sittingTutorial_ChairRotation = new Quaternion(0, 0.999537826f, 0, 0.0303991847f);

    public Vector3 standingTutorial_ChairPosition = new Vector3(20.34f, 0.69f, -18.75f);
    public Quaternion standingTutorial_ChairRotation = new Quaternion(0, 0.6f, 0, 0.79f);


    public Vector3 girlSittingSpawn = new Vector3(-18.8799992f, -13.8900003f, 38.3699989f);
    public Vector3 girlWaiting = new Vector3(-12.5100002f, -13.5699997f, 41.0200005f);

    public void Setup(TutorialState state)
    {
        switch (state)
        {
            case TutorialState.Intro:
                IntroTutorial();
                break;
            case TutorialState.Sitting:
                SittingTutorial();
                break;
            case TutorialState.Standing:
                StandingTutorial();
                break;
        }
    }

    void IntroTutorial()
    {
        rain.Play();
        inventoryBag.SetActive(false);
        detectiveBook.SetActive(false);
        detect.enabled = false;
        player.PlayAnimation("Standing");
        SetAsMainCamera(cameraTutorial1);
    }

    void SittingTutorial()
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
    }

    void StandingTutorial()
    {
        detectiveBook.SetActive(true);
        SetAsMainCamera(player.GetComponentInChildren<Camera>());
        boyPhotograph.SetActive(true);
        girlChair.transform.SetLocalPositionAndRotation(standingTutorial_ChairPosition, standingTutorial_ChairRotation);
        girlSprite.transform.position = girlWaiting;
        if (girlSprite.TryGetComponent(out Animator animator))
        {
            animator.Play("GirlStanding");
        }

        player.PlayAnimation("Idle");
        player.transform.position = standingSpawn;
    }

    public void FirstInspectionOccurred()
    {
        SetupInventoryInvoker();
    }

    void SetupInventoryInvoker()
    {
        inventoryBag.SetActive(true);
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
