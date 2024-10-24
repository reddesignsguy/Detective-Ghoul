using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo -- possibly rename to something more intuitive
public struct ParsedInventoryItem
{
    public InventoryItem item;
    public string code;

    public ParsedInventoryItem(InventoryItem item, string code)
    {
        this.item = item;
        this.code = code;
    }
}

public class InventorySystem : MonoBehaviour
{

    List<ParsedInventoryItem> items;

    private void Awake()
    {
        items = new List<ParsedInventoryItem>();
    }

    private void OnEnable()
    {
        EventsManager.instance.onPickUpItem += HandlePickUpItem;
    }

    private void OnDisable()
    {
        EventsManager.instance.onPickUpItem -= HandlePickUpItem;
    }

    private void HandlePickUpItem (Item item)
    {
        Debug.Log("Picking up itemmmm");

        InventoryItem baseData = item.ItemInfo;
        string unlockCode = item.GetLockID();

        ParsedInventoryItem finalData = new ParsedInventoryItem(baseData, unlockCode);

        items.Add(finalData);

        Debug.Log("test");

        Debug.Log(items[0].item._name);
        Destroy(item.transform.gameObject);
    }
}
