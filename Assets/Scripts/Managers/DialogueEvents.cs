using DS.ScriptableObjects;
using System;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    public static DialogueEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // If an instance already exists, destroy this one
        }
        else
        {
            instance = this;
        }
    }

    public event Action<DSDialogueSO> onDialogueStarted;

    public void StartDialogue(DSDialogueSO d)
    {
        if (onDialogueStarted != null)
        {
            onDialogueStarted(d);
        }

    }
}