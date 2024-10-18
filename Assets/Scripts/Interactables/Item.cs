using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo - potentially move this into an inventory manager
public enum ItemType
{
    Key
}

public class Item : MonoBehaviour, Interactable
{
    [SerializeField] private InventoryItem itemInfo;
    [SerializeField] private GameObject lockReference;

    public InventoryItem ItemInfo => itemInfo;

    public string GetLockID()
    {
        if (lockReference == null)
            return null;

        return lockReference.GetInstanceID().ToString();
    }

    public string GetSuggestion()
    {
        return "Pick up " + itemInfo.name;
    }

    public void Interact()
    {
        EventsManager.instance.PickupItem(this);
    }
}
