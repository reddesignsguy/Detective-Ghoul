using System.Collections.Generic;
using UnityEngine;

public class IntercablesDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask interactableLayer;
    private GameObject lastDetectedObject;

    private void Update()
    {
        DetectClosestInteractable();
    }

    private void DetectClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer);
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

        if (closestObject != null && closestObject != lastDetectedObject)
        {
            if (lastDetectedObject != null)
            {
                EventsManager.instance.ToggleableDetect(false, lastDetectedObject);
            }

            EventsManager.instance.ToggleableDetect(true, closestObject);
            lastDetectedObject = closestObject;
        }
        else if (closestObject == null && lastDetectedObject != null)
        {
            EventsManager.instance.ToggleableDetect(false, lastDetectedObject);
            lastDetectedObject = null;
        }
    }

    public GameObject GetLastDetectedObject()
    {
        return lastDetectedObject;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
