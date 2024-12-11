using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintUIManager : UIManager
{
    public TextMeshProUGUI tmp; 
    public Animator animator;

    private void Update()
    {
        if (panel.activeSelf)
        {
            if ((Input.GetKeyDown(KeyCode.F)) && isCooledDown())
            {
                CloseUI();
            }

        }
    }
    private void OnEnable()
    {
        EventsManager.instance.onHint += OnHandleHint;
    }

    private void OnDisable()
    {
        EventsManager.instance.onHint -= OnHandleHint;
    }

    private void OnHandleHint(string hint)
    {
        SetUIActive(true);

        tmp.text = hint;
    }

    public void CloseUI()
    {
        SetUIActive(false);
    }
}