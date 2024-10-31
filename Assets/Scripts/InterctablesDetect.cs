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
            //Debug.Log("hit" + hit.gameObject);
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = hit.gameObject;
            }
        }

        EventsManager.instance.ToggleableDetect(closestObject);
        Debug.Log(closestObject);
        lastDetectedObject = closestObject;
    }

    public GameObject GetLastDetectedObject()
    {
        return lastDetectedObject;
    }

  
}
