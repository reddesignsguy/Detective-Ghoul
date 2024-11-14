using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;

public class PageUI : OptionsDialogUI
{
    public int numPlaceholder;
    public TextMeshProUGUI pageNumPlaceholder;

    public override void SetUp(List<DSDialogueSO> newDialogues)
    {
        base.SetUp(newDialogues);
        pageNumPlaceholder.text = numPlaceholder.ToString();
    }
}
