using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{

    public static GameContext Instance { get; private set; }
    public ContextState state { get; private set; } = ContextState.UI;
    private ContextState lastState = ContextState.UI;
    
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
        bool tryingToExitTutorial = newState == ContextState.FreeRoam || newState == ContextState.UI;
        if (tryingToExitTutorial && inTutorialState() && !inFinalTutorialState())
        {
            return;
        }

        if (newState == state)
        {
            return;
        }

        HandleTransition(newState);

        lastState = state;
        state = newState;
    }

    private void HandleTransition(ContextState newState)
    {
        bool anyToZoom = state != ContextState.Zoomed && newState == ContextState.Zoomed;
        if (anyToZoom)
        {
            EventsManager.instance.ToggleZoom(true);
        }

        bool zoomToAny = state == ContextState.Zoomed && newState != ContextState.Zoomed;
        if (zoomToAny)
        {
            EventsManager.instance.ToggleZoom(false);
        }
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
    UI,
    Zoomed
}