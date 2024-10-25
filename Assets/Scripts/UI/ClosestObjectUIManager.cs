using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosestObjectUIManager : UIManager
{
    public GameObject interactableUIPrefab;

    int currentClosestObjectID;
    GameObject currentClosestObjectUI;

    private void Awake()
    {
        currentClosestObjectID = -1;
        currentClosestObjectUI = null;
    }

    private void OnEnable()
    {
        EventsManager.instance.OnToggleableDetect += HandleToggleableDetect;
    }

    private void OnDisable()
    {
        EventsManager.instance.OnToggleableDetect -= HandleToggleableDetect;
    }

    private void HandleToggleableDetect(GameObject interactableGameObject)
    {
        ShowInteractableUI(interactableGameObject);
    }

    private void ShowInteractableUI(GameObject interactableGameObject)
    {
        if (interactableGameObject == null)
        {
            Destroy(currentClosestObjectUI);
            currentClosestObjectID = -1;
            return;
        }

        bool alreadyShowingUI = interactableGameObject.GetInstanceID() == currentClosestObjectID;
        if (alreadyShowingUI)
        {
            return;
        }

        GameObject uiElement = GenerateUI(interactableGameObject);

        // Replace last closest's objects UI if exists
        if (currentClosestObjectUI != null)
            Destroy(currentClosestObjectUI);

        currentClosestObjectID = interactableGameObject.GetInstanceID();
        currentClosestObjectUI = uiElement;
    }

    /* Attaches a UI to the closest object*/
    private GameObject GenerateUI(GameObject interactableGameObject)
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

        return uiElement;
    }
}
