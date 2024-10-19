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

    /* Marks the start of key selection when attempting to unlock a locked toggleable*/
    public event Action<LockedToggleable> onStartKeySelection;

    public void StartKeySelection(LockedToggleable toggleable)
    {
        if (onStartKeySelection != null)
        {
            onStartKeySelection(toggleable);
        }
    }

    /* Marks the end of key selection whether or not the player has chosen a key for unlocking a locked toggleable*/
    public event Action<LockedToggleable> onEndKeySelection;

    public void EndKeySelection(LockedToggleable toggleable)
    {
        if (onEndKeySelection != null)
        {
            onEndKeySelection(toggleable);
        }
    }

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

    public event Action<bool, GameObject> OnToggleableDetect;

    public void ToggleableDetect(bool show, GameObject interactableGameObject)
    {
        if (OnToggleableDetect != null)
        {
            OnToggleableDetect(show, interactableGameObject);
        }
    }



}