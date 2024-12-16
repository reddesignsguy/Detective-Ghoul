using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public struct InventoryUIReferences
{
    public List<Image> itemImages;
    public List<Image> placeholderImages;
    public List<Button> buttons;
    public List<Image> basePlaceholderImages;

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
    [SerializeField] private Color selectedColor = Color.black;
    [SerializeField] private Color unselectedColor;
    public GameObject defaultHolder;
    public Animator animator;
    public Image animationObject;

    private Color defaultColor;

    private InventoryUIReferences refs;

    private Queue<InventoryItem> insertItems;

    private void Awake()
    {
        refs = new InventoryUIReferences();
        refs.itemImages = GetItemImages();
        refs.placeholderImages = GetItemPlaceholders();
        refs.buttons = GetButtons();
        refs.basePlaceholderImages = GetItemBasePlaceholders();
        refs.viewedItemIndex = 0;
        SetupButtons();

        defaultColor = GetDefaultHolderColor();

        insertItems = new Queue<InventoryItem>();
    }

    private void OnEnable()
    {
        SetupToolbar();
        SelectItem(refs.viewedItemIndex);
        EventsManager.instance.onAddedToInventory += HandleAddedToInventory;
        EventsManager.instance.onNoUIManagersDisplayed += HandleNoUIManagersDisplayed;
    }

    private void OnDisable()
    {
        EventsManager.instance.onNoUIManagersDisplayed -= HandleNoUIManagersDisplayed;
        EventsManager.instance.onAddedToInventory -= HandleAddedToInventory;
    }

    private void HandleNoUIManagersDisplayed()
    {
        if (insertItems.Count != 0)
        {
            InventoryItem item = insertItems.Dequeue();
            SetupAnimation(item);
        }

        insertItems.Clear();
    }


    private void HandleAddedToInventory(InventoryItem item)
    {
        insertItems.Enqueue(item);
    }

    public override void SetUIActive(bool open)
    {
        base.SetUIActive(open);
        if (open)
        {
            SetupToolbar();
            SelectItem(refs.viewedItemIndex);
        }
    }

    private void SetupAnimation(InventoryItem item)
    {
        animationObject.sprite = item.image;
        animator.SetTrigger("PickedUp");
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

        GameObject placeholder = refs.basePlaceholderImages[index].gameObject;
        SetShadow(refs.viewedItemPlaceholder, false);
        refs.viewedItemPlaceholder = placeholder;
        refs.viewedItemIndex = index;

        // Update view
        ViewItem(GetItem(index));
        SetShadow(placeholder, true);
    }

    // assumes that the slot has an item
    private void SetShadow(GameObject placeholder, bool set)
    {
        Image img = placeholder?.GetComponent<Image>();
        if (img)
        {
            if (set)
            {
                img.color = selectedColor;
            }
            else
            {
                img.color = unselectedColor;
            }
        }
    }

    private void ViewItem(InventoryItem item)
    {
        if (item == null)
        {
            title.text = "";
            description.text = "";
            viewedItemImage.enabled = false;
            return;
        }


        title.text = item.name;
        description.text = item.description;
        viewedItemImage.sprite = item.image;
        viewedItemImage.enabled = true;
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

    private List<Image> GetItemBasePlaceholders()
    {
        List<Image> images = new List<Image>(toolbar.GetComponentsInChildren<Image>());
        images = images.FindAll(image => image.gameObject.name == "Base");
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

    private Color GetDefaultHolderColor()
    {
        if (defaultColor == null)
        {
            defaultColor = 
                defaultHolder.GetComponent<Image>().color;
        }

        Debug.Log(defaultColor);
        return defaultColor;
    }
}
