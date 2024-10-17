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
    [SerializeField] private string codeID;

    public ItemType ItemID => itemID;
    public string CodeID => codeID;

    public void Interact()
    {
        EventsManager.instance.PickupItem(this);
    }
}
