using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectUIManager : UIManager
{
    private Animator animator;
    private InventoryItem item = null;
    private GameObject go = null;

    public Image image;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public Controls itemControls;

    private void Awake()
    {
        animator = panel.GetComponent<Animator>();
    }


    private void Update()
    {
        if (panel.activeSelf)
        {
            // pick up
            if ( (Input.GetKeyDown(KeyCode.F)) && isCooledDown())
            {
                EventsManager.instance.PickUpInventoryItem(item);
                Destroy(go);
                SetUIActive(false);
            }
            // put down
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetUIActive(false);
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

    private void HandleInspect(InventoryItem item, GameObject go)
    {
        SetUp(item, go);
        animator.SetBool("Open", true);
    }


    private void SetUp(InventoryItem item, GameObject go)
    {
        SetUIActive(true);

        // set up photo, name, and description
        image.sprite = item.image;
        title.text = item.name;
        description.text = item.description;

        this.go = go;
        this.item = item;

        EventsManager.instance.ShowControls(itemControls);
    }
}
