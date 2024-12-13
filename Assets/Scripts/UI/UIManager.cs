using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panel;
    private static HashSet<GameObject> openPanels;

    // prevents bug where F automatically closes the UI
    private float creationTime;
    private float cooldownTime = 0.05f;


    private void Awake()
    {
        if (openPanels == null)
            openPanels = new HashSet<GameObject>();
    }

    public virtual void SetUIActive(bool open)
    {
        if (panel != null)
        {
            panel.SetActive(open);

            if (open)
            {
                creationTime = Time.time;

                // disable interactable detect
                openPanels.Add(panel);
                GameContext.Instance.SetContextState(ContextState.UI);
                Debug.Log("State should be UI: " + GameContext.Instance.state);
            }
            else
            {
                // enable
                openPanels.Remove(panel);
            }
        }

        if (openPanels.Count == 0)
            GameContext.Instance.BackOutOfUI();

        print("Panels open during " + GameContext.Instance.state + " state: ");

        foreach (GameObject panel in openPanels)
            print(panel);

        print("----------------");
    }

    protected bool isCooledDown()
    {
        return Time.time - creationTime > cooldownTime;
    }
}
