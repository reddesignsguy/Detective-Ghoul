using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class Clue : Interactee, InventoryItemHolder
{
    public bool affectsGameState;

    [SerializeField] private InventoryItem itemInfo;

    public InventoryItem ItemInfo => itemInfo;

    public InventoryItem GetInventoryItem()
    {
        return itemInfo;
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