using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panel;

    public virtual void SetUIActive(bool open)
    {
        print("Opening: " + panel + "?: " + open);
        if (panel != null)
        {
            panel.SetActive(open);

            if (open)
            {
                // disable interactable detect
                GameContext.Instance.SetContextState(ContextState.UI);
            }
            else
            {
                // enable
                GameContext.Instance.SetContextState(ContextState.FreeRoam);
            }
        }
    }
}
