using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;
using DS.Enumerations;

using UnityEngine.UI;
using DS.Data;

public class QuestionsPageUI : OptionsDialogUI
{
    private int numPlaceholder;
    public TextMeshProUGUI pageNumPlaceholder;

    public List<TextMeshProUGUI> answerPlaceholders;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        DialogueEvents.instance.onSingleDialogueFocused += HandleSingleDialogueFocused;
        DialogueEvents.instance.onMultipleChoiceFocused += HandleMultipleChoiceFocused;
    }

    private void OnDisable()
    {
        DialogueEvents.instance.onSingleDialogueFocused -= HandleSingleDialogueFocused;
        DialogueEvents.instance.onMultipleChoiceFocused -= HandleMultipleChoiceFocused;
    }

    private void HandleMultipleChoiceFocused()
    {
        animator.SetBool("Dim", false);
    }

    private void HandleSingleDialogueFocused()
    {
        animator.SetBool("Dim", true);
    }

    public override void SetUp(DSDialogueSO optionsDialogue)
    {
        if (optionsDialogue == null)
            return;

        if (optionsDialogue.DialogueType != DSDialogueType.MultipleChoice)
            return;

        base.SetUp(optionsDialogue);

        // Get page of  book
        numPlaceholder = DetectiveBook.Instance.GetPageNumber(optionsDialogue);

        SetQuestionsClickable(true);

        // Which options asked?
        for (int i = 0; i < dialogues.Count; i ++)
        {
            DSDialogueChoiceData choice = dialogues[i];

            // Option asked
            Button questionPlaceholder = optionPlaceholders[i];
            if (DialogHistory.Instance.HasVisited(choice.NextDialogue))
            {
                questionPlaceholder.onClick.RemoveAllListeners();

                // Show answer
                answerPlaceholders[i].text = '"' + choice.NextDialogue.Text + '"';

                questionPlaceholder.transition = Selectable.Transition.None;

                // Remove button color
                Image image = questionPlaceholder.GetComponent<Image>();
                if (image)
                {
                    Color color = image.color;
                    color.a = 0;
                    image.color = color;
                }
            }
            else
            {
                answerPlaceholders[i].text = "";
            }
        }
        pageNumPlaceholder.text = "Page " + numPlaceholder.ToString();
    }

    public void SetQuestionsClickable(bool clickable)
    {
        foreach (Button button in optionPlaceholders)
        {
            button.interactable = clickable;

            QuestionUIBehavior behavior = button.gameObject.GetComponent<QuestionUIBehavior>();
            if (behavior)
            {
                behavior.enabled = clickable;
            }

            Image background = button.gameObject.GetComponent<Image>();
            if (background)
            {
                background.enabled = clickable;
            }
        }
    }
}
