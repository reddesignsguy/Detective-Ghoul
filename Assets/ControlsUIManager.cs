using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _controlsBar;
    [SerializeField] private int spacing = 2;

    private void OnEnable()
    {
        EventsManager.instance.onShowControls += HandleShowControls;
    }

    private void OnDisable()
    {
        EventsManager.instance.onShowControls -= HandleShowControls;
    }

    private void HandleShowControls(Controls controls)
    {
        DisplayControls(controls);
    }

    private void DisplayControls(Controls controls)
    {
        string text = "";
        foreach (KeyValuePair<string,string> c in controls)
        {
            text += GetControlText(c);
            text += GetSpacingText();
        }

        _controlsBar.text = text;
    }

    private string GetControlText(KeyValuePair<string, string> c)
    {
        string text = "";
        string keycode = "[" + c.Key + "]";
        string action = c.Value;

        text += keycode + " " + action;
        return text;
    }

    private string GetSpacingText()
    {
        string text = "";
        for (int i = 0; i < spacing; i++)
        {
            text += " ";
        }
        return text;
    }
}
