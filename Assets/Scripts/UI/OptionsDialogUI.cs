using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DS.ScriptableObjects;
using TMPro;

public class OptionsDialogUI : MonoBehaviour
{
    private List<DSDialogueSO> dialogues;
    public List<Button> optionPlaceholders;

    private void Awake()
    {
        dialogues = new List<DSDialogueSO>();
        optionPlaceholders = new List<Button>();
    }

    public virtual void SetUp(List<DSDialogueSO> newDialogues)
    {
        optionPlaceholders.Zip(newDialogues, (Button button, DSDialogueSO dialogue) => {
            if (button.TryGetComponent(out TextMeshProUGUI gui))
            {
                gui.text = dialogue.Text;
            }

            button.onClick.AddListener(() => { });
            return "";
        });

        dialogues = newDialogues;
    }
}
