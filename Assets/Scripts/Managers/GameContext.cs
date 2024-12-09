using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{

    public static GameContext Instance { get; private set; }
    public ContextState state { get; private set; } = ContextState.UI;
    private ContextState stateBeforeLastUI = ContextState.UI;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
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
        HandleState(newState);
        state = newState;
    }

    private void HandleState(ContextState newState)
    {
        switch (newState)
        {
            case ContextState.FreeRoam:
                EventsManager.instance.ShowControls(new Controls() { });
                break;
        }
    }

    // Go back to state before the ui state
    // except if in tutorial
    public void BackOutOfUI()
    {
        Debug.Log("State before last UI" + stateBeforeLastUI);
        if (stateBeforeLastUI == ContextState.StandingTutorial)
        {
            SetContextState(ContextState.FreeRoam);
        }

        else if (state == ContextState.UI)
        {
            state = stateBeforeLastUI;
        }
    }

    private void HandleTransition(ContextState newState)
    {
        bool anyToUI = state != ContextState.UI && newState == ContextState.UI;
        if (anyToUI)
        {
            stateBeforeLastUI = state;
        }

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