using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectUIManager : UIManager
{
    private Animator animator;
    //private InventoryItem item = null;
    private GameObject go = null;

    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private Inspectable inspecting;
    public Controls escControls;

    private void Awake()
    {
        animator = panel.GetComponent<Animator>();
    }


    private void Update()
    {
        if (panel.activeSelf)
        {
            if ( Input.GetKeyDown(KeyCode.F) && isCooledDown())
            {
                inspecting.HandlePressedKeycode(KeyCode.F);
                SetUIActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                inspecting.HandlePressedKeycode(KeyCode.Escape);
                SetUIActive(false);
            }
        }
    }

    private void OnEnable()
    {
        EventsManager.instance.onInspect += HandleInspect;
    }

    private void OnDisable()
    {
        EventsManager.instance.onInspect -= HandleInspect;
    }

    private void HandleInspect(Inspectable inspect, GameObject go)
    {
        SetUp(inspect, go);
        animator.SetBool("Open", true);
    }

    private void SetUp(Inspectable inspect, GameObject go)
    {
        SetUIActive(true);

        this.go = go;
        inspecting = inspect;

        // set up photo, name, and description
        InspectableInfo info = inspect.GetInfo();
        image.sprite = info.Image;
        title.text = info.Name;
        description.text = info.Description;

        EventsManager.instance.ShowControls(new Controls() { info.Controls});
    }
}
