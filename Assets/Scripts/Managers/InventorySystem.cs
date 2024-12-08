using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo -- possibly rename to something more intuitive
public struct ParsedInventoryItem
{
    public InventoryItem item;
    //public string code;

    public ParsedInventoryItem(InventoryItem item, string code)
    {
        this.item = item;
        //this.code = code;
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
        EventsManager.instance.onPickUpItem += HandlePickUpItem;
        EventsManager.instance.onPickUpInventoryItem += HandlePickUpInventoryItem;
    }


    private void OnDisable()
    {
        EventsManager.instance.onPickUpItem -= HandlePickUpItem;
        EventsManager.instance.onPickUpInventoryItem -= HandlePickUpInventoryItem;

    }

    private void HandlePickUpInventoryItem(InventoryItem item)
    {
        if (item == null || item.isClue)
            return;

        ParsedInventoryItem finalData = new ParsedInventoryItem(item, "Obsolete");

        items.Add(finalData);
    }

    [Obsolete]
    public void HandlePickUpItem (Item item)
    {
        if (item == null)
            return;

        Debug.Log("Picking up");
        InventoryItem baseData = item.ItemInfo;
        string unlockCode = item.GetLockID();

        ParsedInventoryItem finalData = new ParsedInventoryItem(baseData, unlockCode);

        items.Add(finalData);
        Destroy(item.transform.gameObject);
    }

    public void Remove(InventoryItem item)
    {
        items.RemoveAll(candidate => candidate.item == item);
    }
}
