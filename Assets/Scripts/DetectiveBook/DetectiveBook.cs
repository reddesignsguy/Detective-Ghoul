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

    private HashSet<InventoryItem> pickupedItemClues;

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
        EventsManager.instance.onPickUpClue += HandlePickupClue;
    }

    private void OnDisable()
    {
        EventsManager.instance.onPickUpClue -= HandlePickupClue;
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
        //List<QuestionsPage> dialogPages = pages.OfType<QuestionsPage>().ToList();

        //for (int i = 0; i < dialogPages.Count; i ++)
        //{
        //    QuestionsPage page = dialogPages[i];

        //    if (page.optionsDialogue == dialogue)
        //    {
        //        return i + 1;
        //    }
        //}

        throw new System.Exception("Trying to get the page number of a dialog that doesn't exist");
    }

    private void HandlePickupClue(Clue clue)
    {
        InventoryItem item = clue.ItemInfo;

        // Remember that we picked up this item
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

    private void turnToPage(int pageNum) // 0 indexed
    {
        Page cur = pages[pageNum];

        if (cur.GetType() == typeof(PicturesPage))
        {
            PicturesPage page = (PicturesPage)cur;
            SetupPicturesPage(page);
        }
    }

    private void SetupPicturesPage(PicturesPage page)
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (InventoryItem needed in page.clues)
        {
            if (pickupedItemClues.Contains(needed))
            {
                sprites.Add(needed.image);
            }
        }


    }
}

[CreateAssetMenu(fileName = "PicturesPage", menuName = "DetectiveBook/PicturesPage")]
public class PicturesPage : Page
{
    public List<InventoryItem> clues;
}