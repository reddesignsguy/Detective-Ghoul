using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIHelper : MonoBehaviour
{
    public InventoryUIManager uiManager;
    public Animator animator;

    private bool on = false;

    public void OnBagClick()
    {
        on = !on;

        uiManager.SetUIActive(on);
        animator.SetBool("On", on);
    }
}
