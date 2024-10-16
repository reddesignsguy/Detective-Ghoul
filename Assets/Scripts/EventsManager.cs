using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // If an instance already exists, destroy this one
        }
        else
        {
            instance = this;
        }

    }

    public event Action<LockedToggleable> onStart;

    public void StartUnlocking(LockedToggleable toggleable)
    {
        if (onStart != null)
        {
            onStart(toggleable);
        }
    }

    public event Action<string> onUnlockAttempt;

    public void AttemptUnlock(String code)
    {
        if (onUnlockAttempt != null)
        {
            onUnlockAttempt(code);
        }
    }
}