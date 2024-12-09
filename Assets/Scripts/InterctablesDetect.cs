using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class IntercablesDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask interactableLayer;

    // Zooming in interactables todo -- refactor
    public Canvas canvas;
    public RectTransform magnifyingGlass;
    public RectTransform identifierRect;
    public int gridStep = 10; // smaller = more raycasts

    public float completelyIdentifiedThreshold = 95f;
    public float partiallyIdentifiedThreshold = 50f;

    public Controls inspectControls;
    public Controls zoomControls;

    private HashSet<Interactee> visitedHiddenInteractees;
    private GameObject lastDetectedObject;

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
        Dictionary<Interactee, HashSet<Vector2>> raycastHitsByInteractee;
        ProcessHits(hits, out potentialClosest, out raycastHitsByInteractee);

        Rect rect = GetMagnifyingGlassRect();
        if (potentialClosest != null)
        {
            HashSet<Vector2> hitPositions = raycastHitsByInteractee[potentialClosest];
            RectHelper.ModifyRect(hitPositions, identifierRect);

            float percentage = GetPercentageOfBoundsCovered(identifierRect, magnifyingGlass);
            bool closestObjectInView = ContainsAtLeastOnePoint(hitPositions, rect);
            bool visitedClosestInteractable = visitedHiddenInteractees.Contains(potentialClosest);
            if ((visitedClosestInteractable && closestObjectInView) || (percentage > completelyIdentifiedThreshold && ContainsAllPoints(hitPositions, rect)))
            {
                if (!visitedClosestInteractable)
                    visitedHiddenInteractees.Add(potentialClosest);

                EventsManager.instance.ZoomInObject(hitPositions, rect, potentialClosest.GetSuggestion());
                EventsManager.instance.ShowControls(new Controls() { inspectControls});
                lastDetectedObject = potentialClosest.gameObject;
            }
            else if (percentage > partiallyIdentifiedThreshold && closestObjectInView)
            {
                EventsManager.instance.ShowControls(new Controls() { });
                EventsManager.instance.ZoomInObject(hitPositions, rect, "???");
                lastDetectedObject = null;
            }
            else
            {
                EventsManager.instance.ShowControls(new Controls() { });
                EventsManager.instance.ZoomInObject(null, Rect.zero, "");
                lastDetectedObject = null;
            }
        }
        else
        {
            EventsManager.instance.ShowControls(new Controls() { });
            EventsManager.instance.ZoomInObject(null, Rect.zero, "");
            lastDetectedObject = null;
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

    public Rect GetMagnifyingGlassRect()
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
