using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Controls : IEnumerable<KeyValuePair<string, string>>
{
    [SerializeField] private List<string> keycodes;
    [SerializeField] private List<string> actions;

    public Controls(List<Controls> manyControls)
    {
        keycodes = new List<string>();
        actions = new List<string>();

        foreach (Controls controls in manyControls)
        {
            keycodes.AddRange(controls.keycodes);
            actions.AddRange(controls.actions);
        }
    }

    public Controls()
    {
        keycodes = new List<string>();
        actions = new List<string>();
    }

    public IEnumerable<KeyValuePair<string, string>> GetControls()
    {
        return keycodes.Zip(actions, (key, value) => new KeyValuePair<string, string>(key, value));
    }

    public void Add(Controls controls)
    {
        keycodes.AddRange(controls.keycodes);
        actions.AddRange(controls.actions);
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return keycodes.Zip(actions, (key, value) => new KeyValuePair<string, string>(key, value)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}