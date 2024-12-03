using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panel;
    private static HashSet<GameObject> openPanels;

    private void Awake()
    {
        if (openPanels == null)
            openPanels = new HashSet<GameObject>();
    }

    public virtual void SetUIActive(bool open)
    {
        print("Opening: " + panel + "?: " + open);
        if (panel != null)
        {
            panel.SetActive(open);

            if (open)
            {
                // disable interactable detect
                openPanels.Add(panel);
                GameContext.Instance.SetContextState(ContextState.UI);
            }
            else
            {
                // enable
                openPanels.Remove(panel);
            }
        }

        if (openPanels.Count == 0)
            GameContext.Instance.SetContextState(ContextState.FreeRoam);

        print("Panels open: ");
        foreach (GameObject panel in openPanels)
            print(panel);

        print("-------------");

    }
}
