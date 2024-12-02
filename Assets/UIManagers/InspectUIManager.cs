using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectUIManager : UIManager
{
    private Animator animator;
    private InventoryItem item = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            // pick up
            if ( Input.GetKeyDown(KeyCode.F))
            {
                EventsManager.instance.PickUpInventoryItem(item);
                panel.SetActive(false);
            }
            // put down
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                panel.SetActive(false);
            }

        }
    }

    private void OnEnable()
    {
        EventsManager.instance.onInspect += HandleInspect;
    }

    private void OnDisable()
    {
        EventsManager.instance.onInspect -= HandleInspect;
    }

    private void HandleInspect(InventoryItem item)
    {
        SetUp(item);
        animator.SetBool("Open", true);
    }


    private void SetUp(InventoryItem item)
    {
        // set up photo, name, and description
        
        this.item = item;
    }
}
