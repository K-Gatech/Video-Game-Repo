using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MazeTrigger : MonoBehaviour
{
    Renderer mazeRenderer;
    GameObject player;
    GameObject key;

    private float totalTime;
    private float timeRemaining;
    public GameObject textDisplay;

    bool puzzleStarted;

    private void Start()
    {
        mazeRenderer = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        puzzleStarted = false;
        totalTime = 90;
        timeRemaining = totalTime;
        key = GameObject.FindGameObjectWithTag("KeyDesert");
    }

    private void Update()
    {
        if (SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.ReachedMaze)
        {
            if (mazeRenderer.bounds.Contains(player.transform.position))
            {
                if (!puzzleStarted)
                {
                    puzzleStarted = true;
                    textDisplay.GetComponent<Text>().text = "00:" + timeRemaining;
                }
                else
                {
                    if (timeRemaining > 0)
                    {
                        timeRemaining -= Time.deltaTime;
                        textDisplay.GetComponent<Text>().text = timeRemaining.ToString("0") + " seconds left";
                    }
                    else
                    {
                        textDisplay.GetComponent<Text>().text = "";

                        player.transform.position = SaveGameDesert.GetSavedPlayerPosition();
                        puzzleStarted = false;
                        timeRemaining = totalTime;
                        SceneManager.LoadScene("DesertPlanet");
                    }
                }
            }
            else
            {
                if (puzzleStarted)
                {
                    textDisplay.GetComponent<Text>().text = "";
                    puzzleStarted = false;
                    timeRemaining = totalTime;
                }
            }
        }
        else
        {
            if (key == null || !key.activeSelf)
            {
                textDisplay.GetComponent<Text>().text = "";
            }
        }
    }
}
