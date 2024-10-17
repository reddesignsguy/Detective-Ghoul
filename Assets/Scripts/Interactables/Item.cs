using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    [SerializeField] public string itemID { get; private set;  }
    [SerializeField] public string codeID { get; private set; }

    public void Interact()
    {
        EventsManager.instance.PickupItem(this);
    }
}
