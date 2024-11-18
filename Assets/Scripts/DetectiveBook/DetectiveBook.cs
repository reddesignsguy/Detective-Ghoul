using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using System.Linq;
using DS.Data;
using TMPro;
using System;

public class DetectiveBook : MonoBehaviour
{
    public List<Page> pages;

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
        //List<> dialogPages = pages.OfType<QuestionsPage>().ToList();

        //for
        //throw new NotImplementedException();
    }
}

public class PicturesPage : Page
{
    public List<InventoryItem> clues;
}