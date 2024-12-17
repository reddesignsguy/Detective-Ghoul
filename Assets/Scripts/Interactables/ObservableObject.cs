using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableObject : Interactee, Interactable, Inspectable
{
    [SerializeField] private ItemData itemInfo;

    private void Awake()
    {
        List<string> keycodes = new List<string>() {"ESC" };
        List<string> actions = new List<string>() { "Leave" };

        inspectControls = new Controls(keycodes, actions);
    }

    public override void Interact()
    {
        EventsManager.instance.Inspect(this, gameObject);

        if (affectsGameState)
        {
            EventsManager.instance.NotifyImportantInteraction(this);
        }
    }

    public InspectableInfo GetInfo()
    {
        return new InspectableInfo(itemInfo.image, itemInfo.name, itemInfo.description, inspectControls);
    }

    public void HandlePressedKeycode(KeyCode code)
    {
        if (code == KeyCode.Escape)
        {
            // todo -- call PutDown item event
        }
    }

}
