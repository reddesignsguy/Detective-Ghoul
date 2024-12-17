using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DS.ScriptableObjects;
using System;
using System.Collections;

public class DialogueUIManager : UIManager
{
    private DSDialogueSO dialogue;
    public Text dialogueText;
    public float fillOutRate = 14.0f;
    private Coroutine fillOutcoroutine;
    public Image spritePlaceholder;
    public HorizontalLayoutGroup layout;
    public float maxSpriteWidth = 400f;
    public float spriteScreenWidthPercent = 0.2f;

    [Obsolete]
    public void StartDialogueUI(Dialogue dialogue)
    {
        SetUIActive(true);  
        UpdateUI(dialogue);
    }

    public void SetUp(DSDialogueSO dialogue)
    {
        this.dialogue = dialogue;
        dialogueText.text = "";
        StopFillingOut();
        fillOutcoroutine = StartCoroutine(FillOutTextCoroutine());

        SetupSprites(dialogue);

        EventsManager.instance.ShowControls(new Controls { });
    }

    private void SetupSprites(DSDialogueSO dialogue)
    {
        if (dialogue.Sprite == null)
        {
            spritePlaceholder.sprite = null;
            return;
        }

        spritePlaceholder.sprite = dialogue.Sprite;
        SetSprite(dialogue.Sprite);

        if (dialogue.SpriteLeftSide)
        {
            layout.reverseArrangement = false;
        }
        else
        {
            layout.reverseArrangement = true;
        }
    }

    private IEnumerator FillOutTextCoroutine()
    {
        IEnumerator it = dialogue.Text.GetEnumerator();
        while (it.MoveNext())
        {
            dialogueText.text += it.Current;

            // Yield until the next frame
            yield return new WaitForSecondsRealtime(1f/fillOutRate);
        }
        fillOutcoroutine = null;
    }

    [Obsolete]
    public void UpdateUI(Dialogue dialogue)
    {
        dialogueText.text = dialogue.DialogueText;  
    }


    private void Update()
    {

        if (panel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            if (!FillingOutFinished())
            {
                StopFillingOut();
                dialogueText.text = dialogue.Text;
            }
            else
            {
                Skip();
            }

        }
    }

    private bool FillingOutFinished()
    {
        return dialogueText.text == dialogue.Text;
    }

    private void Skip()
    {
        DialogueEvents.instance.StartDialogue(dialogue.Choices[0].NextDialogue);

    }

    public void FinishDialogue(){
         SetUIActive(false);  
    }

    private void StopFillingOut()
    {
        if (fillOutcoroutine != null)
            StopCoroutine(fillOutcoroutine);
        fillOutcoroutine = null;
    }

    private void SetSprite(Sprite sprite)
    {
        spritePlaceholder.sprite = sprite;

        SetSize(sprite);
    }

    private void SetSize(Sprite sprite)
    {
        Vector2 spriteSize = sprite.rect.size;

        RectTransform rectTransform = spritePlaceholder.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            float width = Mathf.Min(maxSpriteWidth, Screen.width * spriteScreenWidthPercent);
            rectTransform.sizeDelta = new Vector2(width, 1f);
        }

        //Rect rect = rectTransform.rect;
        //rect.width = Screen.width * 0.2f;

        AspectRatioFitter fitter = spritePlaceholder.gameObject.GetComponent<AspectRatioFitter>();
        if (fitter != null)
        {
            fitter.aspectRatio = sprite.rect.width / sprite.rect.height;
        }
    }
}
