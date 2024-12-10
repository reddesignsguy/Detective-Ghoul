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

    private void HandleUpdateCursor()
    {
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

        Cursor.visible = false;
    }

    private void HandleNewState(ContextState state)
    {
        if (state != ContextState.FreeRoam)
        {
            SetDefaultCursor();
        }
    }

    private void SetDefaultCursor()
    {
        Cursor.visible = true;
        Cursor.SetCursor(magnifierCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
