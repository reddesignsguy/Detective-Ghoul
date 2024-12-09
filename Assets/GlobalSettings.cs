using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    // Static reference for singleton pattern
    public static GlobalSettings Instance;

    public float freeRoamFOV = 38f;


    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}