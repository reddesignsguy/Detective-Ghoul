using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : UIManager
{

    public GameObject inventoryPanel;
    public InventorySystem inventory;


    private void OpenInventory()
    {
        Debug.Log("finding a key");
        inventoryPanel.SetActive(true);
    }

    

}
