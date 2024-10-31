using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMouvement player;

    public enum GameState
    {
        SittingTutorial,
        StandingTutorial,
        Free
    }

    private GameState state;

    public Vector3 sittingSpawn = new Vector3(-20.84f, -13.69f, 48.38f);
    public Vector3 standingSpawn = new Vector3(-18.85f, -13.4f, 41.29f);


    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.SetMovement(false);
        player.transform.position = sittingSpawn;
        player.PlayAnimation("Sitting");
        state = GameState.SittingTutorial;
    }
}
