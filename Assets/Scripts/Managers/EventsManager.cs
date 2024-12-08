using System;
using UnityEngine;

using DS.ScriptableObjects;
using System.Collections.Generic;

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

    public event Action<Clue> onPickUpClue;

    public void PickupClue(Clue clue)
    {
        if (onPickUpClue != null)
        {
            onPickUpClue(clue);
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

    public event Action<DialogueTrigger> onImportantDialogue;

    public void NotifyImportantDialogueEnded(DialogueTrigger t)
    {
        if (onImportantDialogue != null)
        {
            onImportantDialogue(t);
        }
    }

    public event Action<QuestionUIEvent> onQuestionUIEvent;

    public void NotifyQuestionUIEvent(QuestionUIEvent e)
    {
        if (onQuestionUIEvent != null)
        {
            onQuestionUIEvent(e);
        }
    }

    public event Action<GameObject> onExclusiveUIOpened;

    public void OpenUIExclusively(GameObject go)
    {
        if (onExclusiveUIOpened != null)
        {
            onExclusiveUIOpened(go);
        }
    }

    public event Action<InventoryItem, GameObject> onInspect;

    public void Inspect(InventoryItem inventoryItem, GameObject obj)
    {
        if (onInspect != null)
        {
            onInspect(inventoryItem, obj);
        }
    }

    public event Action<InventoryItem> onPickUpInventoryItem;

    public void PickUpInventoryItem(InventoryItem item)
    {
        if (onPickUpInventoryItem != null)
        {
            onPickUpInventoryItem(item);
        }
    }

    public event Action<HashSet<Vector2>, Rect, string> onHighlightArea;

    public void ZoomInObject(HashSet<Vector2> points, Rect bounds, string hint)
    {
        if (onHighlightArea != null)
        {
            onHighlightArea(points, bounds, hint);
        }
    }

    public event Action<bool> onToggleZoom;

    public void ToggleZoom(bool toggle)
    {
        if (onToggleZoom != null)
        {
            onToggleZoom(toggle);
        }
    }
}