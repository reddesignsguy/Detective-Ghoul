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

    public event Action<LockedToggleable> onStartUnlocking;

    public void StartUnlocking(LockedToggleable toggleable)
    {
        if (onStartUnlocking != null)
        {
            onStartUnlocking(toggleable);
        }
    }

    public event Action<LockedToggleable> onAttemptUnlock;

    public void AttemptUnlock(LockedToggleable toggleable)
    {
        if (onAttemptUnlock != null)
        {
            onAttemptUnlock(toggleable);
        }
    }
}