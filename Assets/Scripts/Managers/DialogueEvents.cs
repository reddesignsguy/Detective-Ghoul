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

    public event Action<DSDialogueSO> onDialogueFinished;

    public void FinishDialogue(DSDialogueSO d)
    {
        if (onDialogueFinished != null)
        {
            onDialogueFinished(d);
        }

    }

    public event Action onExitedOptions;

    public void ExitOptions()
    {
        if (onExitedOptions != null)
        {
            onExitedOptions();
        }

    }

    public event Action onSingleDialogueFocused;

    public void SingleDialogueFocused()
    {
        if (onSingleDialogueFocused != null)
        {
            onSingleDialogueFocused();
        }
    }

    public event Action onMultipleChoiceFocused;

    public void MultipleChoiceFocused()
    {
        if (onMultipleChoiceFocused != null)
        {
            onMultipleChoiceFocused();
        }
    }
}