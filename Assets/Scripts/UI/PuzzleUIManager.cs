using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleUIManager : UIManager
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetUIActive(false);
        }
    }

    public override void SetUIActive(bool open)
    {
        base.SetUIActive(open);
        if (open)
        {
            //EventsManager.instance.ShowControls(controls);
        }
    }
}
