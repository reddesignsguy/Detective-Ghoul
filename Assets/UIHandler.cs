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


    private void Awake()
    {
        //inventoryPanel.SetActive(false);
         DialoguePanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

   private void OnEnable()
    {
        EventsManager.instance.onStartKeySelection += HandleStartKeySelection;
        EventsManager.instance.OnToggleableDetect += HandleToggleableDetect;
        EventsManager.instance.onStartDialogue += ShowDialogue;
    }

    private void OnDisable()
    {
        EventsManager.instance.onStartKeySelection -= HandleStartKeySelection;
        EventsManager.instance.OnToggleableDetect -= HandleToggleableDetect;
        EventsManager.instance.onStartDialogue -= ShowDialogue;
    }

    private void HandleStartKeySelection(LockedToggleable toggleable)
    {
        Debug.Log("finding a key");
        inventoryPanel.SetActive(true);
    }

    private void HandleToggleableDetect(bool show, GameObject interactableGameObject)
    {
        if (show)
        {
            ShowInteractableUI(interactableGameObject);
        }
        else
        {
            HideInteractableUI(interactableGameObject);
        }
    }

    private void ShowInteractableUI(GameObject interactableGameObject)
    {
        if (!uiElements.ContainsKey(interactableGameObject))
        {
            GameObject uiElement = Instantiate(interactableUIPrefab, interactableGameObject.transform.position + Vector3.up * 2f, Quaternion.identity);
            string suggestion = interactableGameObject.GetComponent<Interactable>().GetSuggestion();
            RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one * 0.01f; 
            }
            Canvas canvas = uiElement.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.worldCamera = Camera.main; 
            }

            Text uiText = uiElement.GetComponentInChildren<Text>();
            
            if (uiText != null)
            {
                uiText.text = suggestion;
            }
            
            uiElements[interactableGameObject] = uiElement;
        }
}


        private void HideInteractableUI(GameObject interactableGameObject)
        {
            if (uiElements.TryGetValue(interactableGameObject, out GameObject uiElement))
            {
                Destroy(uiElement);
                uiElements.Remove(interactableGameObject);
            }
        
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


    private void ShowDialogue(Dialogue dialogue)
    {
        DialoguePanel.SetActive(true);
        DialogueText.text = dialogue.DialogueText;
        SetOptions(dialogue.options);
    }


    public void SetOptions(List<Option> options)
    {
        for (int i = 0; i < OptionButtons.Count; i++)
        {
            if (i < options.Count)
            {
                OptionButtons[i].gameObject.SetActive(true);
                OptionButtons[i].GetComponentInChildren<Text>().text = options[i].OptionText;

                int optionId = options[i].id;
                OptionButtons[i].onClick.RemoveAllListeners();
                OptionButtons[i].onClick.AddListener(() => OnOptionClicked(optionId));
            }
            else
            {
                OptionButtons[i].gameObject.SetActive(false);
            }
        }
    }
     private void OnOptionClicked(int optionId)
    {
        FindObjectOfType<DialogueManager>().OnOptionSelected(optionId);
    }

    public void HideDialogue()
    {
        DialoguePanel.SetActive(false);
    }
    void Update()
    {
        
    }
}
