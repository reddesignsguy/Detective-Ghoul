using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

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
        Destroy(item.transform.gameObject);
    }
}
