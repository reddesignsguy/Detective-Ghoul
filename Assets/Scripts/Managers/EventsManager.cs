using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance { get; private set; }

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

    ////////////////////// ////////////////////// ////////////////////// ////////////////////// //////////////////////
    //// UI
    /* Marks the start of key selection when attempting to unlock a locked toggleable*/
    public event Action onOpenInventory;

    public void OpenInventory()
    {
        if (onOpenInventory != null)
        {
            onOpenInventory();
        }
    }

    /* Marks the end of key selection whether or not the player has chosen a key for unlocking a locked toggleable*/
    public event Action onCloseInventory;

    public void CloseInventory()
    {
        if (onCloseInventory != null)
        {
            onCloseInventory();
        }
    }

    public event Action<GameObject> OnToggleableDetect;

    public void ToggleableDetect(GameObject interactableGameObject)
    {
        if (OnToggleableDetect != null)
        {
            OnToggleableDetect(interactableGameObject);
        }
    }

    public event Action<Dialogue> onStartDialogue;
    public void StartDialogue(Dialogue dialogue)
    {
        if (onStartDialogue != null)
        {
            onStartDialogue(dialogue);
        }
    }

    public event Action<bool> onSetMovement;
    public void SetMovement(bool on)
    {
        if (onSetMovement != null)
        {
            onSetMovement(on);
        }
    }

    ////////////////////// ////////////////////// ////////////////////// ////////////////////// //////////////////////

    /* Called when the player has chosen a key for unlocking a locked toggleable*/
    public event Action<string> onUnlockAttempt;

    public void AttemptUnlock(string code)
    {
        if (onUnlockAttempt != null)
        {
            onUnlockAttempt(code);
        }
    }

    public event Action<Item> onPickUpItem;

    public void PickupItem(Item item)
    {
        if (onPickUpItem != null)
        {
            onPickUpItem(item);
        }
    }

    public event Action<Interactee> onImportantInteraction;

    public void NotifyImportantInteraction(Interactee Interactable)
    {
        if (onImportantInteraction != null)
        {
            onImportantInteraction(Interactable);
        }

    }

    public event Action<string> onHint;

    public void ProvideHint(string s)
    {
        if (onHint != null)
        {
            onHint(s);
        }

    }
}