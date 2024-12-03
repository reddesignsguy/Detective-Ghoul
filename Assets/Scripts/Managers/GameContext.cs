using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }
    public ContextState state { get; private set; } = ContextState.UI;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameContext found and destroyed.");
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetContextState(ContextState newState)
    {
        Debug.Log("Trying to set state to: " + newState);
        bool tryingToExitTutorial = newState == ContextState.FreeRoam || newState == ContextState.UI;
        if (tryingToExitTutorial && inTutorialState() && !inFinalTutorialState())
        {
            Debug.Log("State change failed");
            return;
        }

        Debug.Log("State change success");
        state = newState;
    }

    private bool inTutorialState()
    {
        return state == ContextState.IntroTutorial || state == ContextState.SittingTutorial || state == ContextState.StandingTutorial; 
    }

    private bool inFinalTutorialState()
    {
        return state == ContextState.StandingTutorial;
    }

}

public enum ContextState
{
    IntroTutorial,
    SittingTutorial,
    StandingTutorial,
    FreeRoam,
    UI
}