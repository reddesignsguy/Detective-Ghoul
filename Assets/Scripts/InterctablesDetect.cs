using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntercablesDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask interactableLayer;
    private GameObject lastDetectedObject;

    // Zooming in interactables
    public Canvas canvas;
    public RectTransform magnifyingGlass;
    public RectTransform identifierRect;
    public int gridStep = 10; // the smaller, the more raycasts (expensive)

    private void Update()
    {
        if (GameContext.Instance.state == ContextState.FreeRoam || GameContext.Instance.state == ContextState.SittingTutorial || GameContext.Instance.state == ContextState.StandingTutorial)
        {
            DetectClosestInteractable();
        }
        else if (GameContext.Instance.state == ContextState.Zoomed)
            DetectClosestHiddenInteractable();
    }

    private void DetectClosestInteractable()
    {

        List<Collider> hits = new List<Collider>(Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer));
        hits = hits.FindAll(collider => !collider.GetComponent<Interactable>().IsHidden()) ;
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = hit.gameObject;
            }
        }

        if (closestObject != null && closestObject.TryGetComponent(out Interactable i) && i.GetSuggestion() == "")
        {
            EventsManager.instance.ToggleableDetect(null);
        }
        else
        {
            EventsManager.instance.ToggleableDetect(closestObject);
        }

        lastDetectedObject = closestObject;
    }

    private void DetectClosestHiddenInteractable()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        float rayDistance = 100f;
        List<FrustrumRaycastInfo> hits = GetInteractablesInView(screenWidth, screenHeight, rayDistance, hidden: true);

        // Organize raycast hits by interactables and find the closest interactable
        GameObject closestObject = null;
        Dictionary<GameObject, HashSet<Vector2>> HiddenObjectsAndRaycastHits = new Dictionary<GameObject, HashSet<Vector2>>();
        float closestDistance = Mathf.Infinity;
        foreach (FrustrumRaycastInfo info in hits)
        {
            Vector2 centerOfScreen = new Vector2(Screen.width, Screen.height);
            float distance = Vector2.Distance(centerOfScreen, info.screenPoint);
            GameObject hitObject = info.hit.collider.gameObject;

            UpdateObjectsAndTheirHits(HiddenObjectsAndRaycastHits, info, hitObject);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = hitObject;
            }
        }

        // Highlight the closest interactable
        if (closestObject != null)
        {
            Rect bounds = GetScreenBounds();
            HashSet<Vector2> pointsOfClosestObject = HiddenObjectsAndRaycastHits[closestObject];
            if (ContainsAllPoints(pointsOfClosestObject, bounds))
            {
                ShowInteractableBounds(pointsOfClosestObject);
                SetupInteractableHint(closestObject.GetComponent<Interactable>()?.GetSuggestion());
            }
            else if (ContainsAtLeastOnePoint(pointsOfClosestObject, bounds))
            {
                RemoveOutOfBoundPoints(pointsOfClosestObject, bounds);
                ShowInteractableBounds(pointsOfClosestObject);
                SetupInteractableHint("???");
            }
            else
            {
                HideInteractableBounds();
            }
        }

        lastDetectedObject = closestObject;
    }


    private void UpdateObjectsAndTheirHits(Dictionary<GameObject, HashSet<Vector2>> objectsAndTheirHits, FrustrumRaycastInfo info, GameObject hitObject)
    {
        if (!objectsAndTheirHits.ContainsKey(hitObject))
        {
            objectsAndTheirHits[hitObject] = new HashSet<Vector2>();
        }
        objectsAndTheirHits[hitObject].Add(info.screenPoint);
    }

    // Gets all the interactables that player can see
    private List<FrustrumRaycastInfo> GetInteractablesInView(int screenWidth, int screenHeight, float rayDistance, bool hidden = true)
    {
        List<FrustrumRaycastInfo> hits = new List<FrustrumRaycastInfo>();
        for (int x = 0; x < screenWidth; x += gridStep)
        {
            for (int y = 0; y < screenHeight; y += gridStep)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
                {
                    Vector3 hitPoint = Camera.main.WorldToScreenPoint(hit.point);
                    FrustrumRaycastInfo info = new FrustrumRaycastInfo(hit, hitPoint);
                    hits.Add(info);
                }
            }
        }
        hits = hits.FindAll(collider => collider.hit.collider.gameObject.GetComponent<Interactable>().IsHidden() == hidden);
        return hits;
    }

    private void ShowInteractableBounds(HashSet<Vector2> points)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (var point in points)
        {
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        Vector2 position = new Vector2((minX + maxX) / 2.0f, (minY + maxY) / 2.0f);
        Vector2 size = new Vector2(maxX - minX, maxY - minY);

        SetInteractableBounds(position, size.x, size.y);
        identifierRect.gameObject.SetActive(true);
    }

    private void SetInteractableBounds(Vector2 position, float width, float height)
    {

        Vector2 normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
        Vector2 normalizedSize = new Vector2(width / Screen.width, height / Screen.height);

        // Uses normalized coordinates
        identifierRect.anchorMin = new Vector2(normalizedPosition.x - (normalizedSize.x / 2.0f),
                                              normalizedPosition.y - (normalizedSize.y / 2.0f));
        identifierRect.anchorMax = new Vector2(normalizedPosition.x + (normalizedSize.x / 2.0f),
                                              normalizedPosition.y + (normalizedSize.y / 2.0f));

        identifierRect.anchoredPosition = Vector2.zero; // Position is handled by anchors
        identifierRect.sizeDelta = Vector2.zero;        // Size is handled by anchors
    }

    private void SetupInteractableHint(String s)
    {
        TextMeshProUGUI UGUI = identifierRect.GetComponentInChildren<TextMeshProUGUI>();
        UGUI.text = s;
    }

    private void HideInteractableBounds()
    {
        identifierRect.gameObject.SetActive(false);
    }

    private bool ContainsAllPoints(HashSet<Vector2> points, Rect bounds)
    {
        foreach(Vector2 point in points)
        {
            if (!bounds.Contains(point))
                return false;
        }

        return true;
    }

    private bool ContainsAtLeastOnePoint(HashSet<Vector2> points, Rect bounds)
    {
        foreach (Vector2 point in points)
        {
            if (bounds.Contains(point))
                return true;
        }

        return false;
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

    public Rect GetScreenBounds()
    {
        float scaleFactor = canvas.GetComponent<CanvasScaler>().scaleFactor;

        float rectWidth = (magnifyingGlass.anchorMax.x - magnifyingGlass.anchorMin.x) * Screen.width * scaleFactor;
        float rectHeight = (magnifyingGlass.anchorMax.y - magnifyingGlass.anchorMin.y) * Screen.height * scaleFactor;

        Vector2 anchorPosition = new Vector2(
            magnifyingGlass.anchorMin.x * Screen.width * scaleFactor,
            magnifyingGlass.anchorMin.y * Screen.height * scaleFactor
        );

        Vector2 position = anchorPosition;

        return new Rect(position, new Vector2(rectWidth, rectHeight));
    }

    public struct FrustrumRaycastInfo
    {
        public RaycastHit hit;
        public Vector2 screenPoint;

        public FrustrumRaycastInfo(RaycastHit hit, Vector2 screenPoint)
        {
            this.hit = hit;
            this.screenPoint = screenPoint;
        }
    }

    public GameObject GetLastDetectedObject()
    {
        return lastDetectedObject;
    }

}
