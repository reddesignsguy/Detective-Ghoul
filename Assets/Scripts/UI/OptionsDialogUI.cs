using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DS.ScriptableObjects;
using TMPro;

public class OptionsDialogUI : UIManager
{
    protected List<DSDialogueSO> dialogues;
    public List<Button> optionPlaceholders;

    private void Awake()
    {
        dialogues = new List<DSDialogueSO>();
        optionPlaceholders = new List<Button>();
    }

    public virtual void SetUp(DSDialogueSO optionsDialogue)
    {
        List<DSDialogueSO> options = optionsDialogue.Choices.Select((DS.Data.DSDialogueChoiceData choice) => {return choice.NextDialogue; }).ToList();

        optionPlaceholders.Zip(options, (Button button, DSDialogueSO option) => {
            // Option text
            if (button.TryGetComponent(out TextMeshProUGUI gui))
            {
                gui.text = option.Text;
            }

            // Notify listeners when option chosen
            DSDialogueSO resultDialogue = option.Choices[0].NextDialogue;
            button.onClick.AddListener(() => EventsManager.DialogueEvents.instance.StartDialogue(resultDialogue)) ;
            return "";
        });

        dialogues = options;
    }
}
