using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInvoker : MonoBehaviour
{
    public UIManager uiManager;
    public Animator animator;

    private bool on = false;

    public void OnInvoke(bool onlyTurnOn = false)
    {
        if (onlyTurnOn)
            on = true;
        else
            on = !on;

        uiManager.SetUIActive(on);
        animator.SetBool("On", on);
    }
}
