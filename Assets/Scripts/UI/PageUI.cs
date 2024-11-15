using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;
using DS.Enumerations;

using UnityEngine.UI;

public class QuestionsPageUI : OptionsDialogUI
{
    private int numPlaceholder;
    public TextMeshProUGUI pageNumPlaceholder;

    public List<TextMeshProUGUI> answerPlaceholders;
    

    public override void SetUp(DSDialogueSO optionsDialogue)
    {
        if (optionsDialogue.DialogueType != DSDialogueType.MultipleChoice)
            return;


        base.SetUp(optionsDialogue);

        // Get page of  book
        numPlaceholder = DetectiveBook.Instance.GetPageNumber(optionsDialogue);

        // Which options asked?
        for (int i = 0; i < dialogues.Count; i ++)
        {
            DSDialogueSO dialogue = dialogues[i];

            // Option asked
            if (DialogHistory.Instance.HasVisited(dialogue))
            {
                Button questionPlaceholder = optionPlaceholders[i];
                questionPlaceholder.onClick.RemoveAllListeners();

                // Show answer
                answerPlaceholders[i].text = dialogue.Choices[0].Text;

                if (questionPlaceholder.TryGetComponent(out TextMeshProUGUI gui))
                {
                    // todo: cross or grey out text, or use a checkbox to show option has been chosen before,
                }
            }
        }
        pageNumPlaceholder.text = numPlaceholder.ToString();
    }
}
