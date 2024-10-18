using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // todo -- possibly rename to something more intuitive
    private struct ParsedInventoryItem
    {
        InventoryItem item;
        string code;

        public ParsedInventoryItem(InventoryItem item, string code)
        {
            this.item = item;
            this.code = code;
        }
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
        Debug.Log("Picking up item");
        InventoryItem baseData = item.ItemInfo;
        string unlockCode = item.LockID;

        ParsedInventoryItem finalData = new ParsedInventoryItem(baseData, unlockCode);

        Destroy(item.transform.gameObject);
    }
}
