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
        GameContext.Instance.ZoomStartEvent += HandleZoomStart;
        GameContext.Instance.ZoomEndEvent += HandleZoomEnd;
        EventsManager.instance.onHighlightArea += HandleHighlightArea;
    }


    private void OnDisable()
    {
        EventsManager.instance.onHighlightArea -= HandleHighlightArea;
    }

    private void HandleZoomStart(Vector3 pos)
    {

        identifyBounds.SetActive(true);
    }

    private void HandleZoomEnd()
    {
        identifyBounds.SetActive(false);
        identifierRect.gameObject.SetActive(false);
    }

    private void HandleHighlightArea(HashSet<Vector2> points, Rect bounds, string hint)
    {
        if (points == null)
        {
            HideHighlightArea();
        }
        else
        {
            ShowHighlightArea(points, bounds, hint);
        }

    }

    private void ShowHighlightArea(HashSet<Vector2> points, Rect bounds, string hint)
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
