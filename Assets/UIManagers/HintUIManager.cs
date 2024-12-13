using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DS.ScriptableObjects;
using DS.Data;

public class HintUIManager : UIManager
{
    public TextMeshProUGUI tmp; 
    public Animator animator;
    public Sprite playerSprite;

    private void OnEnable()
    {
        EventsManager.instance.onHint += OnHandleHint;
    }

    private void OnDisable()
    {
        EventsManager.instance.onHint -= OnHandleHint;
    }


    private void OnHandleHint(string hint)
    {
        DSDialogueSO dialogue = new DSDialogueSO();
        dialogue.InitialiazeDisconnectedDialogue("Hint", hint, playerSprite, true);

        DialogueEvents.instance.StartDialogue(dialogue);
    }
}