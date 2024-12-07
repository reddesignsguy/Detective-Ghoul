using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectZoomUIManager : MonoBehaviour
{
    public RectTransform identifierRect;
    public GameObject identifyBounds;

    private void OnEnable()
    {
        EventsManager.instance.onToggleZoom += HandleToggleZoom;
        EventsManager.instance.onHighlightArea += OnHandleHighlight;
    }


    private void OnDisable()
    {
        EventsManager.instance.onToggleZoom -= HandleToggleZoom;
        EventsManager.instance.onHighlightArea -= OnHandleHighlight;

    }

    private void HandleToggleZoom(bool toggle)
    {
        if (toggle)
        {
            identifyBounds.SetActive(true);
        }
        else
        {
            identifyBounds.SetActive(false);
            identifierRect.gameObject.SetActive(false);
        }
    }

    private void OnHandleHighlight(HashSet<Vector2> points, Rect bounds, string hint)
    {
        if (points == null)
        {
            HideHighlightArea();
        }
        else
        {
            HighlightArea(points, bounds, hint);
        }

    }

    private void HighlightArea(HashSet<Vector2> points, Rect bounds, string hint)
    {
        RemoveOutOfBoundPoints(points, bounds);
        RectHelper.ModifyRect(points, identifierRect);
        SetupInteractableHint(hint);
        identifierRect.gameObject.SetActive(true);
    }

    private void HideHighlightArea()
    {
        identifierRect.gameObject.SetActive(false);
    }

    // Used to only highlight part of the object that is within a certain Rect bounds
    private void RemoveOutOfBoundPoints(HashSet<Vector2> points, Rect bounds)
    {
        HashSet<Vector2> pointsToRemove = new HashSet<Vector2>();

        foreach (Vector2 point in points)
        {
            if (!bounds.Contains(point))
                pointsToRemove.Add(point);
        }

        foreach (Vector2 point in pointsToRemove)
        {
            points.Remove(point);
        }
    }

    private void SetupInteractableHint(string s)
    {
        TextMeshProUGUI UGUI = identifierRect.GetComponentInChildren<TextMeshProUGUI>();
        UGUI.text = s;
    }
}
