using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private InputSystem inputSystem;
    public static GameContext Instance { get; private set; }
    public ContextState state { get; private set; } = ContextState.UI;
    private ContextState stateBeforeLastUI = ContextState.UI;

    public event Action<Vector3> ZoomStartEvent;
    public event Action ZoomEndEvent;
    public event Action ZoomUIEndEvent;
    public event Action ZoomUIStartEvent;
    public event Action<Vector2> PanCameraEvent;
    public event Action<bool> UpdateCursorEvent;
    public event Action<ContextState> EnteredNewStateEvent;

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

    private void OnEnable()
    {
        inputSystem.ZoomInEvent += HandleZoomIn;
        inputSystem.ZoomOutEvent += HandleZoomOut;
        inputSystem.CursorMoveEvent += HandleCursorMove;
    }

    private void HandleZoomIn(Vector2 mousePos)
    {
        if (inputSystem.IsPointerOverUIElement())
        {
            return;
        }

        if (state == ContextState.Zoomed)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            return;
        }

        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 targetPosition = new Vector3(hit.point.x, hit.point.y, Camera.main.transform.position.z);
        if (Vector3.Distance(currentPosition, targetPosition) > GlobalSettings.Instance.maxZoomRoamDistance)
        {
            return;
        }

        SetContextState(ContextState.Zoomed);
        ZoomStartEvent?.Invoke(targetPosition);
        ZoomUIStartEvent?.Invoke();
    }

    private void HandleZoomOut()
    {
        SetContextState(ContextState.FreeRoam);
        ZoomEndEvent?.Invoke();
        ZoomUIEndEvent?.Invoke();
    }

    private void HandleCursorMove(Vector2 mouseDelta)
    {
        switch (state)
        {
            case ContextState.Zoomed:
                PanCameraEvent?.Invoke(mouseDelta);
                break;
            case ContextState.FreeRoam:
                bool hoveringOverUI = inputSystem.IsPointerOverUIElement();
                UpdateCursorEvent?.Invoke(hoveringOverUI);
                break;
        }
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
        state = newState;
    }

    // Go back to state before the ui state
    // except if in tutorial
    public void BackOutOfUI()
    {
        if (stateBeforeLastUI == ContextState.StandingTutorial)
        {
            SetContextState(ContextState.FreeRoam);
        }

        else if (state == ContextState.UI)
        {
            SetContextState(stateBeforeLastUI);
        }
    }

    private void HandleTransition(ContextState newState)
    {
        if (state == newState)
        {
            throw new Exception("Expected the inputted new state to be different from current state");
        }

        switch (newState)
        {
            case ContextState.IntroTutorial:
                EventsManager.instance.ShowControls(new Controls() { });
                break;
            case ContextState.SittingTutorial:
                EventsManager.instance.ShowControls(new Controls() { });
                break;
            case ContextState.StandingTutorial:
                EventsManager.instance.ShowControls(new Controls() { });
                break;
            case ContextState.FreeRoam:
                EventsManager.instance.ShowControls(new Controls() { });
                break;
            case ContextState.UI:
                stateBeforeLastUI = state;
                ZoomUIEndEvent?.Invoke();
                break;
            case ContextState.Zoomed:
                ZoomUIStartEvent?.Invoke();
                break;
        }

        EnteredNewStateEvent?.Invoke(newState);
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