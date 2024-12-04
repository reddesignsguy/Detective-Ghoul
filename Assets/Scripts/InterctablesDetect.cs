using System.Collections.Generic;
using UnityEngine;

public class IntercablesDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask interactableLayer;
    private GameObject lastDetectedObject;

    private void Update()
    {
        if (GameContext.Instance.state == ContextState.FreeRoam || GameContext.Instance.state == ContextState.SittingTutorial || GameContext.Instance.state == ContextState.StandingTutorial)
            DetectClosestInteractable();
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

    private void DetectClosestHiddenInteractable()
    {
        // shoot a boxcast encompassing screen
        Camera cam = Camera.main; // Assign the main camera or another camera
        float rayDistance = 100f; // Max distance of the raycast
        int gridStep = 10; // Reduce the number of rays (smaller = more rays)

        if (cam == null) cam = Camera.main;

        // Get the screen dimensions
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        HashSet<GameObject> hits = new HashSet<GameObject>();

        for (int x = 0; x < screenWidth; x += gridStep)
        {
            for (int y = 0; y < screenHeight; y += gridStep)
            {
                Ray ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
                {
                    hits.Add(hit.collider.gameObject);
                }
            }
        }

        // Print all hit objects
        Debug.Log($"Objects hit by camera frustum rays: {hits.Count}");
        foreach (var obj in hits)
        {
            Debug.Log(obj.name);
        }


        //hits = hits.FindAll(collider => collider.GetComponent<Interactable>().IsHidden());
        //GameObject closestObject = null;
        //float closestDistance = Mathf.Infinity;

        //// get closest hidden interactable
        //foreach (Collider hit in hits)
        //{
        //    float distance = Vector3.Distance(transform.position, hit.transform.position);
        //    if (distance < closestDistance)
        //    {
        //        closestDistance = distance;
        //        closestObject = hit.gameObject;
        //    }
        //}

        //// shoot a boxcast encompassing center of screen, if object is inside the cast, then show an image and name of the interactable
        //// otherwise, show question marks

        //if (closestObject != null && closestObject.TryGetComponent(out Interactable i) && i.GetSuggestion() == "")
        //{
        //    EventsManager.instance.ToggleableDetect(null);
        //}
        //else
        //{
        //    EventsManager.instance.ToggleableDetect(closestObject);
        //}

        //lastDetectedObject = closestObject;
    }

    public GameObject GetLastDetectedObject()
    {
        return lastDetectedObject;
    }

  
}
