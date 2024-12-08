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

    // prevents bug where F automatically closes the UI
    private float creationTime;
    private float cooldownTime = 1f;

    private void Awake()
    {
        animator = panel.GetComponent<Animator>();
    }


    private void Update()
    {
        if (panel.activeSelf)
        {

            // pick up
            if ( (Input.GetKeyDown(KeyCode.F)) && Time.time - creationTime > cooldownTime)
            {
                Debug.Log("Picked up");
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
        creationTime = Time.time;

        // set up photo, name, and description
        image.sprite = item.image;
        title.text = item.name;
        description.text = item.description;

        this.go = go;
        this.item = item;
    }
}
