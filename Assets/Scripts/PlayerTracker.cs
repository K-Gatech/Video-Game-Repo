using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("PlayerBody");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= 100 && SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.TalkedToAlien)
        {
            SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.ReachedMaze);
        }
    }
}
