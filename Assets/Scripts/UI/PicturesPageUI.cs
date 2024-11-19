using System.Collections;
using System.Collections.Generic;
using DS.Data;
using DS.Enumerations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PicturesPageUI : UIManager
{
    public List<Image> imagePlaceholders;

    public TextMeshProUGUI pageNumPlaceholder;

    public void setUp()
    {
        //// Get page of  book
        //numPlaceholder = DetectiveBook.Instance.GetPageNumber(optionsDialogue);

        //// Which options asked?
        //for (int i = 0; i < dialogues.Count; i++)
        //{
        //    DSDialogueChoiceData choice = dialogues[i];

        //    // Option asked
        //    if (DialogHistory.Instance.HasVisited(choice.NextDialogue))
        //    {
        //        Button questionPlaceholder = optionPlaceholders[i];
        //        questionPlaceholder.onClick.RemoveAllListeners();

        //        // Show answer
        //        answerPlaceholders[i].text = choice.NextDialogue.Text;

        //        if (questionPlaceholder.TryGetComponent(out TextMeshProUGUI gui))
        //        {
        //            // todo: cross or grey out text, or use a checkbox to show option has been chosen before,2
        //        }
        //    }
        //}
        //pageNumPlaceholder.text = "Page" + numPlaceholder.ToString();
    }
}
