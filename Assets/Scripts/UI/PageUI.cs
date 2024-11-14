using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;

public class OptionsPageUI : OptionsDialogUI
{
    private int numPlaceholder;
    public TextMeshProUGUI pageNumPlaceholder;
    

    public override void SetUp(DSDialogueSO optionsDialogue)
    {
        base.SetUp(optionsDialogue);

        // Get page of  book
        numPlaceholder = -1;

        pageNumPlaceholder.text = numPlaceholder.ToString();
    }
}
