using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo - potentially move this into an inventory manager
[Obsolete]
public enum ItemType
{
    Key
}

public class Item : Interactee, InventoryItemHolder
{
    public bool affectsGameState;

    [SerializeField] private InventoryItem itemInfo;
    [Obsolete] [SerializeField] private GameObject lockReference;

    public InventoryItem ItemInfo => itemInfo;

    public InventoryItem GetInventoryItem()
    {
        return itemInfo;
    }

    [Obsolete]
    public string GetLockID()
    {
        if (lockReference == null)
            return null;

        return lockReference.GetInstanceID().ToString();
    }

    public override void Interact()
    {
        EventsManager.instance.Inspect(itemInfo, gameObject);

        if (affectsGameState)
        {
            EventsManager.instance.NotifyImportantInteraction(this);
        }
    }

}