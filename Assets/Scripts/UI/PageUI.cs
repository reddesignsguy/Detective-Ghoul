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
    
    public override void SetUp(DSDialogueSO optionsDialogue)
    {
        if (optionsDialogue == null)
            return;

        if (optionsDialogue.DialogueType != DSDialogueType.MultipleChoice)
            return;

        base.SetUp(optionsDialogue);

        // Get page of  book
        numPlaceholder = DetectiveBook.Instance.GetPageNumber(optionsDialogue);

        // Which options asked?
        for (int i = 0; i < dialogues.Count; i ++)
        {
            DSDialogueChoiceData choice = dialogues[i];

            // Option asked
            if (DialogHistory.Instance.HasVisited(choice.NextDialogue))
            {
                Button questionPlaceholder = optionPlaceholders[i];
                questionPlaceholder.onClick.RemoveAllListeners();

                // Show answer
                answerPlaceholders[i].text = choice.NextDialogue.Text;

                if (questionPlaceholder.TryGetComponent(out TextMeshProUGUI gui))
                {
                    // todo: cross or grey out text, or use a checkbox to show option has been chosen before,2
                }
            }
        }
        pageNumPlaceholder.text = numPlaceholder.ToString();
    }
}
