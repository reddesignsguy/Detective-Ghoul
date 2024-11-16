using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    //public DialogueTrigger trigger;
    private DialogueUIManager dialogueUIManager;

    private List<Dialogue> currentDialogues;
    public int currentIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dialogueUIManager = FindObjectOfType<DialogueUIManager>();

    }

    public void StartDialogueSequence(List<Dialogue> dialogues, DialogueTrigger instance)
    {
        currentDialogues = dialogues;
        currentIndex = 0;

        if (currentDialogues.Count > 0)
        {
            StartDialogue(currentDialogues[currentIndex]);
        }

        //trigger = instance;
    }

    private void StartDialogue(Dialogue dialogue)
    {
        if (dialogueUIManager != null)
        {
            dialogueUIManager.StartDialogueUI(dialogue); 
        }
    }

    public void OnOptionSelected(int selectedOptionId)
    {
        Option selectedOption = currentDialogues[currentIndex].options.Find(option => option.id == selectedOptionId);

        if (selectedOption != null)
        {
            currentIndex = selectedOption.NextDialogueIndex;

            if (currentIndex < currentDialogues.Count)
            {
                StartDialogue(currentDialogues[currentIndex]);
            }
            else
            {
                dialogueUIManager.FinishDialogue();
                //EventsManager.instance.NotifyImportantDialogueEnded(trigger);

            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //Skip();
        }
    }

    //public void Skip()
    //{
    //    currentIndex++;

    //    if (currentIndex < currentDialogues.Count) 
    //    {
    //        StartDialogue(currentDialogues[currentIndex]);
    //    }
    //    else 
    //    {
    //        dialogueUIManager.FinishDialogue();
    //        EventsManager.instance.NotifyImportantDialogueEnded(trigger);
    //    }
    //}

}
