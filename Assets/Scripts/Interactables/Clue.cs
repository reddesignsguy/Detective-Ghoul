using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : Interactee
{
    public bool affectsGameState;

    [SerializeField] private InventoryItem itemInfo;

    public InventoryItem ItemInfo => itemInfo;


    public override void Interact()
    {
        transform.gameObject.SetActive(false);

        EventsManager.instance.PickupClue(this);

        if (affectsGameState)
        {
            EventsManager.instance.NotifyImportantInteraction(this);
        }
    }

}