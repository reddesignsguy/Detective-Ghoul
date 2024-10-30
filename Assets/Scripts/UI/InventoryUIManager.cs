using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : UIManager
{

    public InventorySystem inventory;

    private void OnEnable()
    {
        Debug.Log(inventory.items.Count);
        IEnumerator<ParsedInventoryItem> it = inventory.items.GetEnumerator();


        Image[] images = transform.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.gameObject.name == "Holder" || img.gameObject == gameObject)
                continue;

            
            if (it.MoveNext())
            {
                Debug.Log("Picture enabled");
                img.enabled = true;

                ParsedInventoryItem item = it.Current;

                img.sprite = item.item.image;
            }
            else
            {
                img.enabled = false;
                break;
            }
        }
    }

    private void OnDisable()
    {
        Image[] images = transform.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.gameObject.name == "Holder" || img.gameObject == gameObject)
                continue;


            img.enabled = false;
        }
    }

}
