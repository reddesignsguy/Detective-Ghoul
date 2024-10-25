using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : UIManager
{
    public Text dialogueText;              
    public List<Button> OptionButtons;     
    public Button SkipButton;

    public void StartDialogueUI(Dialogue dialogue)
    {
        SetUIActive(true);  
        UpdateUI(dialogue);
        Skip();
    }

    public void UpdateUI(Dialogue dialogue)
    {
        dialogueText.text = dialogue.DialogueText;  
        if(dialogue.options.Count > 0){
            SetOptions(dialogue.options); 
            SkipButton.gameObject.SetActive(false);
        }else {
            SetOptions(dialogue.options); 
            SkipButton.gameObject.SetActive(true);
        }
                        
    }

    private void SetOptions(List<Option> options)
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

    private void Skip(){
         SkipButton.onClick.RemoveAllListeners(); 
         SkipButton.onClick.AddListener(() => FindObjectOfType<DialogueManager>().Skip());
    }

    private void OnOptionClicked(int optionId)
    {
        FindObjectOfType<DialogueManager>().OnOptionSelected(optionId);
    }

    public void FinishDialogue(){
         SetUIActive(false);  
    }
}
