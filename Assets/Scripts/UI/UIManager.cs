using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
        [SerializeField] protected Controls controls;

    public GameObject panel;
    private static HashSet<UIManager> openPanels;

    // prevents bug where F automatically closes the UI
    private float creationTime;
    private float cooldownTime = 0.05f;


    private void Awake()
    {
        if (openPanels == null)
            openPanels = new HashSet<UIManager>();
    }

    public virtual void SetUIActive(bool open)
    {
        if (panel != null)
        {
            panel.SetActive(open);

            if (open)
            {
                creationTime = Time.time;
                openPanels.Add(this);
                GameContext.Instance.SetContextState(ContextState.UI);
                if (controls.Count() != 0)
                {
                    Debug.Log("SHOWING CONTROLS FORM UI MANAGER");
                    EventsManager.instance.ShowControls(controls);
                }
            }
            else
            {
                openPanels.Remove(this);

                // todo -- make an abstraction for managing all panels
                if (openPanels.Count != 0)
                {
                    Controls newControls = openPanels.First().controls;
                    EventsManager.instance.ShowControls(newControls);
                }
            }
        }

        if (openPanels.Count == 0)
        {
            GameContext.Instance.BackOutOfUI();
            EventsManager.instance.NotifyNoUIManagersDisplayed();
        }

        print("Panels open during " + GameContext.Instance.state + " state: ");

        foreach (UIManager panel in openPanels)
            print(panel);

        print("----------------");
    }

    protected bool isCooledDown()
    {
        return Time.time - creationTime > cooldownTime;
    }
}
