using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIOpener : MonoBehaviour, Interactable
{
    public GameObject panel;

    public string GetSuggestion()
    {
        // todo - refactor: make all interactables have a abstract class that takes in an action suggestion
        return "Puzzle";
    }

    public void Interact()
    {
        Debug.Log("Helol");
        panel.SetActive(true);
    }
}
