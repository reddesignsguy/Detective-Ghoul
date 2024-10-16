using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    
    public void Interact()
    {
        EventsManager.instance.PickupItem(this);
    }
}
