using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour
{
    public GameObject endingPanel;
    public AudioSource relaxingMusic;

    private void OnTriggerEnter(Collider other)
    {
        relaxingMusic.Play();
        endingPanel.SetActive(true);
        EventsManager.instance.SetMovement(false);
    }
}

