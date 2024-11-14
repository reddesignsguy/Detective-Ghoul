using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveBook : MonoBehaviour
{

    public static DetectiveBook Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
