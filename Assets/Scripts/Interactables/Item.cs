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
    [SerializeField] private ItemType itemID;
    [SerializeField] private GameObject lockReference;

    public ItemType ItemID => itemID;
    public int LockID => lockReference.GetInstanceID();

    public void Interact()
    {
        EventsManager.instance.PickupItem(this);
    }
}
