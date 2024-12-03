using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintUIManager : MonoBehaviour
{
    public GameObject hintUIPanel;
    public TextMeshProUGUI tmp; 
    public Animator animator;

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
        hintUIPanel.SetActive(true);

        tmp.text = hint;
        //animator.SetTrigger("Show");

        
        EventsManager.instance.SetMovement(false);
    }

    public void CloseUI()
    {
        hintUIPanel.SetActive(false);
        EventsManager.instance.SetMovement(true);

    }

    public bool IsEnabled()
    {
        return hintUIPanel.activeSelf;
    }
}