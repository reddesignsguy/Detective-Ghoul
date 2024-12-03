using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputModule : StandaloneInputModule
{
    private GameObject lastSelectedGameObject = null;
    public override void Process()
    {
        base.Process();

        // Prevent deselection by re-selecting the current GameObject

        if (eventSystem.currentSelectedGameObject == null)
        {

            for (int i = 0; i <= 2; i++) // 0 = left, 1 = right, 2 = middle mouse button
            {
                if (Input.GetMouseButtonDown(i))
                {
                    eventSystem.SetSelectedGameObject(lastSelectedGameObject);
                    break;
                }
            }
        }

        lastSelectedGameObject = eventSystem.currentSelectedGameObject;
    }
}