using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using System.Linq;
using DS.Data;
using TMPro;

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

    public int GetPageNumber(DSDialogueSO dialogue) // 1-indexed
    {
        List<QuestionsPage> dialogPages = pages.OfType<QuestionsPage>().ToList();

        for (int i = 0; i < dialogPages.Count; i ++)
        {
            QuestionsPage page = dialogPages[i];

            if (page.optionsDialogue == dialogue)
            {
                return i + 1;
            }
        }

        throw new System.Exception("Trying to get the page number of a dialog that doesn't exist");
    }

    
}
