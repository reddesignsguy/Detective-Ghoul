using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockedToggleable : Toggleable
{
    
    [SerializeField]
    public List<InventoryItem> keys;
    private bool unlocked = false;


    public override string GetSuggestion()
    {
        return unlocked ? "" : "Unlock";
    }
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
        if (unlocked)
            return;

        List<string> codes = keys.Select(item => item.code).ToList();

        InventorySystem inventory = FindObjectOfType<InventorySystem>();

        List<InventoryItem> itemsToBeRemoved = new List<InventoryItem>();
        foreach (InventoryItem requiredItem in keys)
        {
            string code = requiredItem.code;
            bool exists = inventory.items.Any(item => {
                itemsToBeRemoved.Add(item.item);
                return item.item.code == code;
            });

            if (!exists)
            {
                Debug.Log(requiredItem.hint);

                EventsManager.instance.ProvideHint(requiredItem.hint);
                // Show hint of what is needed
                return;
            }

        }

        // Successfuly unlocked
        foreach(InventoryItem item in itemsToBeRemoved)
        {
            inventory.Remove(item);
        }

        unlocked = true;
        Toggle();
    }

    
}
