using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectZoomUIManager : MonoBehaviour
{
    public InputSystem inputSystem;
    public RectTransform identifierRect;
    public GameObject identifyBounds;
    public Slider slider;

    private void OnEnable()
    {
        GameContext.Instance.ZoomUIStartEvent += HandleZoomStart;
        GameContext.Instance.ZoomUIEndEvent += HandleZoomEnd;
        EventsManager.instance.onHighlightArea += HandleHighlightArea;
        EventsManager.instance.onZoomChange += HandleZoomChange;
    }

    private void OnDisable()
    {
        GameContext.Instance.ZoomUIStartEvent -= HandleZoomStart;
        GameContext.Instance.ZoomUIEndEvent -= HandleZoomEnd;
        EventsManager.instance.onHighlightArea -= HandleHighlightArea;
        EventsManager.instance.onZoomChange -= HandleZoomChange;
    }

    private void HandleZoomStart()
    {
        identifyBounds.SetActive(true);
    }

    private void HandleZoomEnd()
    {
        identifyBounds.SetActive(false);
        identifierRect.gameObject.SetActive(false);
    }

    private void HandleZoomChange(float percent)
    {
        slider.value = percent;
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
