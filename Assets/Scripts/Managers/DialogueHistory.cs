
using DS.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class DialogHistory : MonoBehaviour
{
    public static DialogHistory Instance { get; private set; }

    private HashSet<DSDialogueSO> visited;

    private void Awake()
    {
        // Ensure that only one instance exists
        if (Instance == null)
        {
            Instance = this;
            visited = new HashSet<DSDialogueSO>();

            // Optional: keep this instance persistent across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
    }

    private void OnDisable()
    {
        DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
    }

    private void HandleDialogueStarted(DSDialogueSO dialogue)
    {
        visited.Add(dialogue);
    }

    // Optional method to check if a dialogue has been visited
    public bool HasVisited(DSDialogueSO dialogue)
    {
        return visited.Contains(dialogue);
    }
}
