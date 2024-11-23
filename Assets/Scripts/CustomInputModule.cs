using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
    private GameObject lastSelectedGameObject = null;
    public override void Process()
    {
        base.Process();

        // Prevent deselection by re-selecting the current GameObject
        if (eventSystem.currentSelectedGameObject == null && Input.GetMouseButtonDown(0))
        {
            eventSystem.SetSelectedGameObject(lastSelectedGameObject);
        }

        lastSelectedGameObject = eventSystem.currentSelectedGameObject;
    }
}