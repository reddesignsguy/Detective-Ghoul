using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D magnifierCursor;


    private void OnEnable()
    {
        GameContext.Instance.UpdateCursorEvent += HandleUpdateCursor;
        GameContext.Instance.EnteredNewStateEvent += HandleNewState;
    }

    private void OnDisable()
    {
        GameContext.Instance.UpdateCursorEvent -= HandleUpdateCursor;
        GameContext.Instance.EnteredNewStateEvent -= HandleNewState;
    }

    private void HandleUpdateCursor(bool pointerOverUI)
    {
        if (pointerOverUI)
        {
            SetDefaultCursor();
            return;
        }

        Vector2 mousePos = Mouse.current.position.value;
        
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            SetDefaultCursor();
            return;
        }

        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 targetPosition = new Vector3(hit.point.x, hit.point.y, Camera.main.transform.position.z);
        bool withinBounds = Vector3.Distance(currentPosition, targetPosition) <= GlobalSettings.Instance.maxZoomRoamDistance;
        if (!withinBounds)
        {
            SetDefaultCursor();
            return;
        }

        SetSelectionCursor();
    }

    private void HandleNewState(ContextState state)
    {
        
        if (state == ContextState.Zoomed)
        {
            DisableCursor();
        } else if (state != ContextState.FreeRoam)
        {
            SetDefaultCursor();
        }
    }

    private void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    private void SetSelectionCursor()
    {
        Vector2 hotspot = new Vector2(magnifierCursor.width / 2f, magnifierCursor.height / 2f);
        Cursor.SetCursor(magnifierCursor, hotspot, CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    private void DisableCursor()
    {
        Cursor.visible = false;
    }
}
