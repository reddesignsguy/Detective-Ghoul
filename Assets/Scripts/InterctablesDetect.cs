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

    // Zooming in interactables todo -- refactor
    public Canvas canvas;
    public RectTransform magnifyingGlass;
    public RectTransform identifierRect;
    public int gridStep = 10; // the smaller, the more raycasts (expensive)

    public float completelyIdentifiedThreshold = 95f;
    public float partiallyIdentifiedThreshold = 50f;

    private HashSet<Interactee> visitedHiddenInteractees;

    private void Awake()
    {
        visitedHiddenInteractees = new HashSet<Interactee>();
    }
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
        List<FrustrumRaycastInfo> hits = FindInteracteesInView(Screen.width, Screen.height, rayDistance: 100f, hidden: true);

        Interactee potentialClosest;
        Dictionary<Interactee, HashSet<Vector2>> hiddenObjectsAndRaycastHits;
        ProcessHits(hits, out potentialClosest, out hiddenObjectsAndRaycastHits);

        // Highlight the closest interactable
        if (potentialClosest != null)
        {
            Rect bounds = GetMagnifyingGlassBounds();
            HashSet<Vector2> pointsOfClosestObject = hiddenObjectsAndRaycastHits[potentialClosest];
            RectHelper.ModifyRect(pointsOfClosestObject, identifierRect);

            float percentage = GetPercentageOfBoundsCovered(identifierRect, magnifyingGlass);
            bool closestObjectInView = ContainsAtLeastOnePoint(pointsOfClosestObject, bounds);
            bool visitedClosestInteractable = visitedHiddenInteractees.Contains(potentialClosest);
            if ((visitedClosestInteractable && closestObjectInView) || (percentage > completelyIdentifiedThreshold && ContainsAllPoints(pointsOfClosestObject, bounds)))
            {
                ShowInteractableBounds(pointsOfClosestObject, bounds);
                SetupInteractableHint(potentialClosest.GetSuggestion());

                if (!visitedClosestInteractable)
                    visitedHiddenInteractees.Add(potentialClosest);

                lastDetectedObject = potentialClosest.gameObject;
            }
            else if (percentage > partiallyIdentifiedThreshold && closestObjectInView)
            {
                ShowInteractableBounds(pointsOfClosestObject, bounds);
                SetupInteractableHint("???");
                lastDetectedObject = null;
            }
            else
            {
                HideInteractableBounds();
                lastDetectedObject = null;
            }
        }
    }

    // Finds the closest interactee while also organizing raycast hits with the objects they hit
    private void ProcessHits(List<FrustrumRaycastInfo> hits, out Interactee closestInteractee, out Dictionary<Interactee, HashSet<Vector2>> HiddenObjectsAndRaycastHits)
    {
        closestInteractee = null;
        HiddenObjectsAndRaycastHits = new Dictionary<Interactee, HashSet<Vector2>>();
        float closestDistance = Mathf.Infinity;
        foreach (FrustrumRaycastInfo info in hits)
        {
            Vector2 centerOfScreen = new Vector2(Screen.width, Screen.height);
            float distance = Vector2.Distance(centerOfScreen, info.screenPoint);
            GameObject hitObject = info.hit.collider.gameObject;
            Interactee interactee = hitObject.GetComponent<Interactee>();

            UpdateHiddenObjectsAndRaycastHits(HiddenObjectsAndRaycastHits, info, interactee);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractee = hitObject.GetComponent<Interactee>();
            }
        }
    }

    private void UpdateHiddenObjectsAndRaycastHits(Dictionary<Interactee, HashSet<Vector2>> objectsAndTheirHits, FrustrumRaycastInfo info, Interactee interactee)
    {
        if (!objectsAndTheirHits.ContainsKey(interactee))
        {
            objectsAndTheirHits[interactee] = new HashSet<Vector2>();
        }
        objectsAndTheirHits[interactee].Add(info.screenPoint);
    }

    // Gets all the interactables that player can see
    private List<FrustrumRaycastInfo> FindInteracteesInView(int screenWidth, int screenHeight, float rayDistance, bool hidden = true)
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

    private void ShowInteractableBounds(HashSet<Vector2> points, Rect bounds)
    {
        RemoveOutOfBoundPoints(points, bounds);
        RectHelper.ModifyRect(points, identifierRect);
        identifierRect.gameObject.SetActive(true);
    }

  
    //private void ModifyRect(HashSet<Vector2> points, RectTransform rect)
    //{
    //    float minX = float.MaxValue;
    //    float maxX = float.MinValue;
    //    float minY = float.MaxValue;
    //    float maxY = float.MinValue;

    //    foreach (var point in points)
    //    {
    //        minX = Mathf.Min(minX, point.x);
    //        maxX = Mathf.Max(maxX, point.x);
    //        minY = Mathf.Min(minY, point.y);
    //        maxY = Mathf.Max(maxY, point.y);
    //    }

    //    // Ensure width and height have a minimum value
    //    const float minSizeThreshold = 0.01f;
    //    float width = Mathf.Max(maxX - minX, minSizeThreshold);
    //    float height = Mathf.Max(maxY - minY, minSizeThreshold);

    //    Vector2 position = new Vector2((minX + maxX) / 2.0f, (minY + maxY) / 2.0f);
    //    Vector2 size = new Vector2(width, height);

    //    ModifyRect(position, size.x, size.y, rect);

    //}
    //private void ModifyRect(Vector2 position, float width, float height, RectTransform rect)
    //{
    //    Vector2 normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
    //    Vector2 normalizedSize = new Vector2(width / Screen.width, height / Screen.height);

    //    // Uses normalized coordinates
    //    rect.anchorMin = new Vector2(normalizedPosition.x - (normalizedSize.x / 2.0f),
    //                                          normalizedPosition.y - (normalizedSize.y / 2.0f));
    //    rect.anchorMax = new Vector2(normalizedPosition.x + (normalizedSize.x / 2.0f),
    //                                          normalizedPosition.y + (normalizedSize.y / 2.0f));

    //    rect.anchoredPosition = Vector2.zero; // Position is handled by anchors
    //    rect.sizeDelta = Vector2.zero;        // Size is handled by anchors
    //}

    private void SetupInteractableHint(string s)
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

    public Rect GetMagnifyingGlassBounds()
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

    private float GetPercentageOfBoundsCovered(RectTransform subject, RectTransform area)
    {
        float areaSize = area.rect.width * area.rect.height;
        float subjectSize = subject.rect.width * subject.rect.height;
        Debug.Log(subjectSize / areaSize);
        return subjectSize / areaSize;
    }

    public GameObject GetLastDetectedObject()
    {
        return lastDetectedObject;
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

}
