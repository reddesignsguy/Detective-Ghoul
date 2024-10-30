using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockedToggleable : Toggleable
{
    
    [SerializeField]
    public List<InventoryItem> keys;

    //private void Awake()
    //{
    //    code = "";
    //}

    //private void OnEnable()
    //{
    //    EventsManager.instance.onUnlockAttempt += OnHandleUnlockAttempt;
    //}

    //private void OnDisable()
    //{
    //    EventsManager.instance.onUnlockAttempt -= OnHandleUnlockAttempt;
    //}

    //private void OnHandleUnlockAttempt (string code)
    //{
    //    if (this.code == code)
    //    {
    //        Toggle();
    //    }
    //}


    public override void Interact()
    {
        Debug.Log("Interaction");
        List<string> codes = keys.Select(item => item.code).ToList();

        InventorySystem inventory = FindObjectOfType<InventorySystem>();
        foreach (InventoryItem requiredItem in keys)
        {
            string code = requiredItem.code;
            Debug.Log("Need code: " + code);
            bool exists = inventory.items.Any(item => {
                return item.item.code == code; });

            if (!exists)
            {
                Debug.Log(requiredItem.hint);

                // Show hint of what is needed
                return;
            }
        }

        Debug.Log("Toggled!");
        // We have what is required
        Toggle();
    }

    
}
