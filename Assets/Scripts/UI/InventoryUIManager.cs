using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct InventoryUIReferences
{
    public List<Image> itemImages;
    public List<Image> placeholderImages;
    public List<Button> buttons;

    public int viewedItemIndex;
    public GameObject viewedItemPlaceholder;
}

public class InventoryUIManager : UIManager
{

    public InventorySystem inventory;
    public GameObject toolbar;
    public Image viewedItemImage;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private InventoryUIReferences refs;

    private void Awake()
    {
        refs = new InventoryUIReferences();
        refs.itemImages = GetItemImages();
        refs.placeholderImages = GetItemPlaceholders();
        refs.buttons = GetButtons();
        refs.viewedItemIndex = 0;
        SetupButtons();
    }

    private void OnEnable()
    {
        SetupToolbar();
        SelectItem(refs.viewedItemIndex);
    }


    private void SetupToolbar()
    {
        IEnumerator<ParsedInventoryItem> it = inventory.items.GetEnumerator();

        // Set up tool bar
        foreach (Image img in refs.itemImages)
        {
            // Fill slot with next item
            if (it.MoveNext())
            {
                Debug.Log(it.Current.item);
                ParsedInventoryItem item = it.Current;
                img.enabled = true;
                img.sprite = item.item.image;
            }
            else
            {
                img.enabled = false;
                break;
            }
        }
    }

    private void SelectItem(int index)
    {
        Debug.Log("Click detected. Selecting item at index: " + index + " when there are " + GetNumberOfItems() + " items");
        if (index >= GetNumberOfItems())
        {
            return;
        }
        Debug.Log("Should be selecting item");

        GameObject placeholder = refs.placeholderImages[index].gameObject;
        SetShadow(refs.viewedItemPlaceholder, false);
        refs.viewedItemPlaceholder = placeholder;
        refs.viewedItemIndex = index;

        // Update view
        ViewItem(GetItem(index));
        SetShadow(placeholder, true);
    }

    private void SetShadow(GameObject placeholder, bool set)
    {
        Shadow shadow = placeholder?.GetComponent<Shadow>();
        if (shadow)
            shadow.enabled = set;
    }

    private void ViewItem(InventoryItem item)
    {
        title.text = item.name;
        description.text = item.description;
        viewedItemImage.sprite = item.image;
    }

    private void SetupButtons()
    {
        for (int i = 0; i < refs.buttons.Count; i++)
        {
            int index = i;
            refs.buttons[i].onClick.AddListener(() => SelectItem(index));
        }
    }

    private List<Image> GetItemImages()
    {
        List<Image> images = new List<Image>(toolbar.GetComponentsInChildren<Image>());
        images = images.FindAll(image => image.gameObject.name == "ItemImage");
        return images;
    }

    private List<Image> GetItemPlaceholders()
    {
        List<Image> images = new List<Image>(toolbar.GetComponentsInChildren<Image>());
        images = images.FindAll(image => image.gameObject.name == "Holder");
        return images;
    }

    private List<Button> GetButtons()
    {
        List<Button> buttons = new List<Button>(toolbar.GetComponentsInChildren<Button>());
        return buttons;
    }

    private int GetNumberOfItems()
    {
        return inventory.items.Count;
    }

    private InventoryItem GetItem(int index)
    {
        if (index >= GetNumberOfItems())
        {
            return null;
        }

        return inventory.items[index].item;
    }
}
