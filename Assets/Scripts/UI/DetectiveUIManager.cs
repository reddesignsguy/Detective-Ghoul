using System;
using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DetectiveBookUIManager : UIManager
{
    public GameObject bulletpointPrefab;
    public PicturesPageUI picturesUI;
    public QuestionsPageUI questionsUI;

    [SerializeField] private Controls navigation;

    private int numPages = -1;
    private int curPageNum = 0;
    private bool isOpen = false;

    private void Awake()
    {
        numPages = DetectiveBook.Instance.pages.Count;
        for (int i = 0; i < numPages; i ++)
        {
            GameObject bulletPoint = Instantiate(bulletpointPrefab, transform);

            if(i == 0 && bulletPoint.TryGetComponent(out BulletPoint point))
            {
                point.SetFill(true);
            }

            bulletPoint.gameObject.SetActive(false);
        }
    }

    public override void SetUIActive(bool open)
    {
        if (open)
        {
            isOpen = true;
            SetBulletPointsEnabled(true);
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
            base.SetUIActive(open);

            EventsManager.instance.ShowControls(navigation);
        }
        else
        {
            isOpen = false;
            SetBulletPointsEnabled(false);
            base.SetUIActive(false);
        }
    }

    private Page SetupPage(int pageNum) // 0 indexed
    {
        Page cur = DetectiveBook.Instance.pages[pageNum];
        print("On page: " + pageNum);

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
        questionsUI.SetQuestionsClickable(false);
    }

    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && curPageNum > 0)
        {
            Close();
            turnPage(goToRightPage: false);
            Open();
        }
        else if (Input.GetKeyDown(KeyCode.E) && curPageNum < numPages - 1)
        {
            Close();
            turnPage(goToRightPage: true);
            Open();
        }
    }

    private void turnPage(bool goToRightPage)
    {
        List<BulletPoint> bulletPoints = new List<BulletPoint>(GetComponentsInChildren<BulletPoint>(true));
        if (bulletPoints.Count != numPages)
        {
            throw new Exception("Discrepancy between number of pages and the number of bullet points for detective book! NumPages: " + numPages + "Number of bullet points: " + bulletPoints.Count);
        }

        // Hollow out old page
        bulletPoints[curPageNum].SetFill(false);

        if (goToRightPage)
        {
            curPageNum += 1;
        }
        else
        {
            curPageNum -= 1;
        }

        // Fill in new page
        bulletPoints[curPageNum].SetFill(true);
    }

    private void Close()
    {
        SetUIActive(false);
    }

    private void Open()
    {
        SetUIActive(true);
    }

    private void SetBulletPointsEnabled(bool enable)
    {
        List<BulletPoint> bulletPoints = new List<BulletPoint>(GetComponentsInChildren<BulletPoint>(true));
        foreach(BulletPoint point in bulletPoints)
        {
            if (enable)
                point.gameObject.SetActive(true);
            else
                point.gameObject.SetActive(false);
        }
    }
}
