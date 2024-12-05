using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntercablesDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask interactableLayer;
    private GameObject lastDetectedObject;

    public Canvas canvas;
    public RectTransform magnifyingGlass;
    public RectTransform identifierRect;

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
            //Debug.Log("hit" + hit.gameObject);
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

    public int gridStep = 10;
    private void DetectClosestHiddenInteractable()
    {
        float scaleFactor = canvas.GetComponent<CanvasScaler>().scaleFactor;

        // shoot a boxcast encompassing screen
        Camera cam = Camera.main; // Assign the main camera or another camera
        float rayDistance = 100f; // Max distance of the raycast
        Debug.Log("Scale factor: " + scaleFactor);

        if (cam == null) cam = Camera.main;

        // Get the screen dimensions
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        List<FrustrumRaycastInfo> hits = new List<FrustrumRaycastInfo>();

        for (int x = 0; x < screenWidth; x += gridStep)
        {
            for (int y = 0; y < screenHeight; y += gridStep)
            {
                Vector3 originPoint = new Vector3(x, y, 0);
                Ray ray = cam.ScreenPointToRay(originPoint);
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
                {
                    Vector3 hitPoint = Camera.main.WorldToScreenPoint(hit.point);
                    FrustrumRaycastInfo info = new FrustrumRaycastInfo(hit, hitPoint);
                    hits.Add(info);
                }
            }
        }


        hits = hits.FindAll(collider => collider.hit.collider.gameObject.GetComponent<Interactable>().IsHidden());
        GameObject closestObject = null;
        Vector2 closestPoint = Vector2.zero;
        HashSet<Vector2> allPointsOfClosestObject = new HashSet<Vector2>();
        float closestDistance = Mathf.Infinity;

        // get closest hidden interactable
        foreach (FrustrumRaycastInfo info in hits)
        {
            Vector2 centerOfScreen = new Vector2(Screen.width, Screen.height);
            float distance = Vector2.Distance(centerOfScreen, info.screenPoint);

            GameObject hitObject = info.hit.collider.gameObject;

            // Track all points to the closest object
            if (hitObject == closestObject)
            {
                allPointsOfClosestObject.Add(info.screenPoint);
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = info.screenPoint;

                // New closest object
                if (hitObject != closestObject)
                {
                    allPointsOfClosestObject.Clear();
                    allPointsOfClosestObject.Add(info.screenPoint);
                }

                closestObject = hitObject;
            }
        }

        if (closestObject != null )
        {
            Rect bounds = GetScreenBounds();
            Debug.Log("# of points " + allPointsOfClosestObject.Count);

            ShowInteractableBounds(allPointsOfClosestObject);
            if (ContainsAllPoints(allPointsOfClosestObject, bounds))
            {
                ShowInteractableBounds(allPointsOfClosestObject);
                Debug.Log("$Closest object inside bounds: " + closestObject);
            }
            else if (ContainsAtLeastOnePoint(allPointsOfClosestObject, bounds))
            {
                RemoveExcludedPoints(allPointsOfClosestObject, bounds);
                ShowInteractableBounds(allPointsOfClosestObject);
                Debug.Log("$Closest object PARTIALLY inside bounds: " + closestObject);
            }
            else
            {
                HideInteractableBounds();
                Debug.Log("$Closest object: " + closestObject);
            }
        }

        lastDetectedObject = closestObject;
    }

    private void ShowInteractableBounds(HashSet<Vector2> points)
    {
        // Initialize the position, width, and height variables
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        // Loop through all points to find the min and max X, Y values
        foreach (var point in points)
        {
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        // Calculate the position, width, and height
        Vector2 position = new Vector2((minX + maxX) / 2.0f, (minY + maxY) / 2.0f);
        Vector2 size = new Vector2(maxX - minX, maxY - minY);

        SetInteractableBounds(position, size.x, size.y);
        identifierRect.gameObject.SetActive(true);
    }

    private void SetInteractableBounds(Vector2 position, float width, float height)
    {
        Vector2 normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
        Vector2 normalizedSize = new Vector2(width / Screen.width, height / Screen.height);

        // Set anchors to normalized coordinates
        identifierRect.anchorMin = new Vector2(normalizedPosition.x - (normalizedSize.x / 2.0f),
                                              normalizedPosition.y - (normalizedSize.y / 2.0f));
        identifierRect.anchorMax = new Vector2(normalizedPosition.x + (normalizedSize.x / 2.0f),
                                              normalizedPosition.y + (normalizedSize.y / 2.0f));

        // Set position and size
        identifierRect.anchoredPosition = Vector2.zero; // Position is handled by anchors
        identifierRect.sizeDelta = Vector2.zero;        // Size is handled by anchors
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


    private void RemoveExcludedPoints(HashSet<Vector2> points, Rect bounds)
    {
        HashSet<Vector2> pointsToRemove = new HashSet<Vector2>();

        foreach (Vector2 point in points)
        {
            if (!bounds.Contains(point))
                pointsToRemove.Add(point);
        }

        // Now remove all points in pointsToRemove from the original collection
        foreach (Vector2 point in pointsToRemove)
        {
            points.Remove(point);
        }
    }
    public GameObject GetLastDetectedObject()
    {
        return lastDetectedObject;
    }

    public Rect GetScreenBounds()
    {
        float scaleFactor = canvas.GetComponent<CanvasScaler>().scaleFactor;

        // Get width and height in screen space
        float rectWidth = (magnifyingGlass.anchorMax.x - magnifyingGlass.anchorMin.x) * Screen.width * scaleFactor;
        float rectHeight = (magnifyingGlass.anchorMax.y - magnifyingGlass.anchorMin.y) * Screen.height * scaleFactor;

        // Get position of the bottom-left corner of the RectTransform
        Vector2 anchorPosition = new Vector2(
            magnifyingGlass.anchorMin.x * Screen.width * scaleFactor,
            magnifyingGlass.anchorMin.y * Screen.height * scaleFactor
        );

        // Adjust for pivot (pivot is at the center: 0.5, 0.5)
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

}
