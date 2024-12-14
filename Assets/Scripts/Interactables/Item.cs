using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

// todo - potentially move this into an inventory manager
[Obsolete]
public enum ItemType
{
    Key
}

public class Item : Interactee, InventoryItemHolder, Inspectable
{

    [SerializeField] private InventoryItem itemInfo;

    public InventoryItem ItemInfo => itemInfo;


    private void Awake()
    {
        List<string> keycodes = new List<string>() { "F", "ESC"};
        List<string> actions = new List<string>() { "Pick up", "Leave"};

        inspectControls = new Controls(keycodes, actions);
    }

    public InventoryItem GetInventoryItem()
    {
        return itemInfo;
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
        // pick up
        if (code == KeyCode.F)
        {
            EventsManager.instance.PickUpInventoryItem(itemInfo);
            Destroy(gameObject);
        }
        // put down
        else if (code == KeyCode.Escape)
        {
            EventsManager.instance.LeftInventoryItem(itemInfo);
        }
    }

}