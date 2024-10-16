using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        Destroy(item.transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
