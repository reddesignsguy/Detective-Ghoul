using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : UIManager
{
    public Text dialogueText;
    public List<Button> OptionButtons;


    private void UpdateUI(Dialogue dialogue)
    {
        dialogueText.text = dialogue.DialogueText;
        SetOptions(dialogue.options);
    }


    public void SetOptions(List<Option> options)
    {
        for (int i = 0; i < OptionButtons.Count; i++)
        {
            if (i < options.Count)
            {
                OptionButtons[i].gameObject.SetActive(true);
                OptionButtons[i].GetComponentInChildren<Text>().text = options[i].OptionText;

                int optionId = options[i].id;
                OptionButtons[i].onClick.RemoveAllListeners();
                OptionButtons[i].onClick.AddListener(() => OnOptionClicked(optionId));
            }
            else
            {
                OptionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnOptionClicked(int optionId)
    {
        FindObjectOfType<DialogueManager>().OnOptionSelected(optionId);
    }
}
