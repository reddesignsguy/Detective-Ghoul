using System;
using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;

public class DetectiveBookUIManager : UIManager
{
    public PicturesPageUI picturesUI;
    public QuestionsPageUI questionsUI;

    private int numPages = -1;
    private int curPageNum = 0;

    public override void SetUIActive(bool open)
    {
        if (open)
        {
            Page page = SetupPage(curPageNum);
            switch (page)
            {
                case PicturesPage:
                    panel = picturesUI.gameObject;
                    break;
                case QuestionsPage:
                    panel = questionsUI.gameObject;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported page type: {page.GetType()}");
            }
            numPages = DetectiveBook.Instance.pages.Count;

            base.SetUIActive(open);
        }
        else
        {
            base.SetUIActive(false);
        }
    }

    private Page SetupPage(int pageNum) // 0 indexed
    {
        Page cur = DetectiveBook.Instance.pages[pageNum];

        switch (cur)
        {
            case PicturesPage page:
                SetupPicturesPage(page, pageNum);
                break;
            case QuestionsPage page:
                SetupQuestionsPage(page);
                break;
            default:
                throw new InvalidOperationException($"Unsupported page type: {cur.GetType()}");
        }

        return cur;
    }

    private void SetupPicturesPage(PicturesPage page, int pageNum)
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (InventoryItem needed in page.clues)
        {
            if (DetectiveBook.Instance.pickupedItemClues.Contains(needed))
            {
                sprites.Add(needed.image);
            }
        }

        picturesUI.setUp(sprites, pageNum);
    }

    private void SetupQuestionsPage(QuestionsPage page)
    {
        DSDialogueSO dialogue = page.optionsDialogue;
        questionsUI.SetUp(dialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            base.SetUIActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && curPageNum > 0)
        {
            base.SetUIActive(false);
            //curPageNum -= 1;
            //base.SetUIActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.E) && curPageNum < numPages - 1)
        {
            base.SetUIActive(false);
            //curPageNum += 1;
            //base.SetUIActive(true);
        }
    }
}
