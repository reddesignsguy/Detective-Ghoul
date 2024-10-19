using System.Collections.Generic;
using UnityEngine;

public class IntercablesDetect : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask interactableLayer;

    private HashSet<GameObject> detectedObjects = new HashSet<GameObject>();

    private void Update()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer);
        HashSet<GameObject> currentHits = new HashSet<GameObject>();

        foreach (Collider hit in hits)
        {
            currentHits.Add(hit.gameObject);

            // Notify that an interactable object is detected
            EventsManager.instance.ToggleableDetect(true, hit.gameObject);
        }

        // Check for objects that are no longer detected
        foreach (var detectedObject in detectedObjects)
        {
            if (!currentHits.Contains(detectedObject))
            {
                // Notify that an interactable object is no longer detected
                EventsManager.instance.ToggleableDetect(false, detectedObject);
            }
        }

        detectedObjects = currentHits; // Update the set of detected objects
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
