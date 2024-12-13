using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using System.Linq;
using DS.Data;
using TMPro;
using System;
using DS;
using static UnityEditor.Progress;
using Unity.VisualScripting;

public class DetectiveBook : MonoBehaviour
{
    public List<Page> pages;

    public HashSet<InventoryItem> pickupedItemClues { get; private set; }

    public static DetectiveBook Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pickupedItemClues = new HashSet<InventoryItem>();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EventsManager.instance.onPickUpInventoryItem += HandlePickupInventoryItem;

    }

    private void OnDisable()
    {
        EventsManager.instance.onPickUpInventoryItem -= HandlePickupInventoryItem;
    }


    public int GetPageNumber(DSDialogueSO dialogue) // 1-indexed
    {
        for (int i = 0; i < pages.Count; i ++)
        {
            Page cur = pages[i];
            if (cur.GetType() == typeof(QuestionsPage))
            {
                QuestionsPage page = (QuestionsPage) cur;

                if (page.optionsDialogue == dialogue)
                {
                    return i + 1;
                }
            }
        }

        throw new System.Exception("Trying to get the page number of a dialog that doesn't exist");
    }


    private void HandlePickupInventoryItem(InventoryItem item)
    {
        if (item == null || !item.isClue)
        {
            return;
        }

        // Store clue by setting it as visited in a hashmap
        for (int i = 0; i < pages.Count; i++)
        {
            Page cur = pages[i];
            if (cur.GetType() == typeof(PicturesPage))
            {
                PicturesPage page = (PicturesPage)cur;
                foreach (InventoryItem needed in page.clues)
                {
                    if (item == needed)
                    {
                        pickupedItemClues.Add(item);
                        return;
                    }
                }
            }
        }
    }
}