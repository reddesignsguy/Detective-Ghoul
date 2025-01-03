using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo -- possibly rename to something more intuitive
public struct ParsedInventoryItem
{
    public InventoryItem item;

    public ParsedInventoryItem(InventoryItem item, string code)
    {
        this.item = item;
    }
}

public class InventorySystem : MonoBehaviour
{

    public List<ParsedInventoryItem> items;

    private void Awake()
    {
        items = new List<ParsedInventoryItem>();
    }

    private void OnEnable()
    {
        EventsManager.instance.onPickupItem += HandlePickUpInventoryItem;
    }


    private void OnDisable()
    {
        EventsManager.instance.onPickupItem -= HandlePickUpInventoryItem;
    }

    private void HandlePickUpInventoryItem(InventoryItem item)
    {
        if (item == null || item.isClue)
            return;

        ParsedInventoryItem finalData = new ParsedInventoryItem(item, "Obsolete");

        items.Add(finalData);

        EventsManager.instance.AddToInventory(item);
        Debug.Log("Sending added to inventory event");

    }

    public void Remove(InventoryItem item)
    {
        items.RemoveAll(candidate => candidate.item == item);
    }
}
