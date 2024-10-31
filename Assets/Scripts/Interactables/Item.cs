using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo - potentially move this into an inventory manager
public enum ItemType
{
    Key
}

public class Item : Interactee
{
    public bool affectsGameState;

    [SerializeField] private InventoryItem itemInfo;
    [SerializeField] private GameObject lockReference;

    public InventoryItem ItemInfo => itemInfo;

    public string GetLockID()
    {
        if (lockReference == null)
            return null;

        return lockReference.GetInstanceID().ToString();
    }

    public override void Interact()
    {
        Debug.Log("interacted with");
        EventsManager.instance.PickupItem(this);

        if (affectsGameState)
        {
            EventsManager.instance.NotifyImportantInteraction(this);
        }
    }

}