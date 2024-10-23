using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public GameObject inventoryPanel;
    public InventorySystem inventory;
    public GameObject interactableUIPrefab;
    private Dictionary<GameObject, GameObject> uiElements = new Dictionary<GameObject, GameObject>();

    public GameObject DialoguePanel;
    public Text DialogueText;
    public List<Button> OptionButtons;


    private void HandleStartKeySelection(LockedToggleable toggleable)
    {
        Debug.Log("finding a key");
        inventoryPanel.SetActive(true);
    }

    //private void MapImages()
    //{

    //    // Clear any existing images in the container.
    //    foreach (Transform child in container)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    // Loop through the sprites list and instantiate images.
    //    foreach (Sprite sprite in sprites)
    //    {
    //        GameObject newImage = Instantiate(imagePrefab, container);  // Create a new Image object.
    //        newImage.GetComponent<Image>().sprite = sprite;  // Set the sprite.
    //    }
    //}

    // Update is called once per frame

}
