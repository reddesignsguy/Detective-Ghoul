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
    public string LockID => lockReference.GetInstanceID().ToString();

    public void Interact()
    {
        Debug.Log("Picking up item");
        EventsManager.instance.PickupItem(this);
    }
}
