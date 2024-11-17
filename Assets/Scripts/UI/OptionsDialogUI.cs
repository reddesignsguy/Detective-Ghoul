using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DS.ScriptableObjects;
using TMPro;
using DS.Data;

public class OptionsDialogUI : UIManager
{
    protected List<DSDialogueChoiceData> dialogues;
    public List<Button> optionPlaceholders;
    private bool exitable = false;

    private void Awake()
    {
        dialogues = new List<DSDialogueChoiceData>();

        if (optionPlaceholders == null)
        {
            optionPlaceholders = new List<Button>();

        }
    }

    public virtual void SetUp(DSDialogueSO optionsDialogue)
    {

        List<DSDialogueChoiceData> options = optionsDialogue.Choices;


        for (int i = 0; i < optionPlaceholders.Count && i < options.Count; i++)
        {
            Button button = optionPlaceholders[i];
            DSDialogueChoiceData option = options[i];

            // Option text
            TextMeshProUGUI gui = button.GetComponentInChildren<TextMeshProUGUI>();

            if (gui)
            {
                gui.text = option.Text;
            }

            // Notify listeners when option chosen
            DSDialogueSO resultDialogue = option.NextDialogue;
            button.onClick.AddListener(() => DialogueEvents.instance.StartDialogue(resultDialogue));
        }

        dialogues = options;

        exitable = optionsDialogue.IsExitable;
    }
}
