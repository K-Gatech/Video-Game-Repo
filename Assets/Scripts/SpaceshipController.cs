using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    public GameObject playerObject;

    private enum OverviewTransitionState
    {
        NotStarted,
        SpaceshipSpawned,
        ReachedPlayer,
        PickedPlayer,
    }

    private OverviewTransitionState currentTransitionState;
    private Vector3 spawnDistance;
    private GameObject player;
    private Player playerScript;

    private float translationSpeed = 0.04f;
    private float rotationSpeed = 0.02f;
    private float maxY = 6;

    private Rigidbody spaceshipBody;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetChild(1).gameObject.SetActive(false);
        player = playerObject.transform.GetChild(1).gameObject;
        playerScript = playerObject.GetComponent<Player>();
        spaceshipBody = GetComponent<Rigidbody>();
        currentTransitionState = OverviewTransitionState.NotStarted;
        spawnDistance = new Vector3(4, 10, 4);
    }

    void Update()
    {
        if (playerScript.isTransitioningToOverviewScreen)
        {
            HandleTransitionToOverviewScreen();
        }
        else
        {
            if (currentTransitionState != OverviewTransitionState.NotStarted)
            {
                OverviewTransitionCleanup();
            }
        }
    }

    private void HandleTransitionToOverviewScreen()
    {
        switch(currentTransitionState)
        {
            case OverviewTransitionState.NotStarted:
                {
                    playerScript.spaceShip.transform.position = playerObject.transform.position + spawnDistance;
                    playerScript.spaceShip.transform.GetChild(0).GetComponent<Renderer>().enabled = true;

                    playerObject.GetComponent<CharacterController>().enabled = false;
                    playerObject.GetComponent<Animator>().enabled = false;

                    currentTransitionState = OverviewTransitionState.SpaceshipSpawned;
                    break;
                }
            case OverviewTransitionState.SpaceshipSpawned:
                {
                    if (FlyToPlayer())
                    {
                        currentTransitionState = OverviewTransitionState.ReachedPlayer;
                    }
                    break;
                }
            case OverviewTransitionState.ReachedPlayer:
                {
                    if (PickPlayerUp())
                    {
                        transform.parent.GetChild(1).gameObject.SetActive(true);
                        playerObject.transform.GetChild(2).gameObject.SetActive(false);
                        playerObject.GetComponent<Player>().enabled = false;

                        currentTransitionState = OverviewTransitionState.PickedPlayer;
                    }
                    break;
                }
            case OverviewTransitionState.PickedPlayer:
                {
                    currentTransitionState = OverviewTransitionState.NotStarted;
                    playerScript.isTransitioningToOverviewScreen = false;

                    // To Be Added
                    player.transform.position = new Vector3(200.74f, 2.63f, -203.1f);
                    PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene("GreenPlanet");
                    break;
                }
        }
    }

    private void OverviewTransitionCleanup()
    {
        currentTransitionState = OverviewTransitionState.NotStarted;

        playerObject.GetComponent<CharacterController>().enabled = true;
        playerObject.GetComponent<Animator>().enabled = true;
        playerScript.spaceShip.SetActive(false);
    }

    private bool PickPlayerUp()
    {
        float newY = Mathf.LerpUnclamped(playerObject.transform.position.y, transform.position.y, 0.01f);

        Vector3 newPosition = new Vector3(playerObject.transform.position.x, newY, playerObject.transform.position.z);
        playerObject.transform.position = newPosition;

        Vector3 newScale = Vector3.LerpUnclamped(playerObject.transform.localScale, new Vector3(0f, 0f, 0f), 0.01f);
        playerObject.transform.localScale = newScale;

        if (newScale.x <= 0.2)
        {
            playerObject.transform.localScale = new Vector3(0f, 0f, 0f);
            return true;
        }

        return false;
    }

    private bool FlyToPlayer()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y + maxY, player.transform.position.z);

        if (!CheckAlmostDoneTranslation(transform.position.y, targetPosition.y))
        {
            float newY = Mathf.LerpUnclamped(transform.position.y, targetPosition.y, translationSpeed);

            Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
            spaceshipBody.MovePosition(newPosition);

            return false;
        }

        RotateTowardsPlayer();

        if (!(CheckAlmostDoneTranslation(transform.position.x, targetPosition.x) && CheckAlmostDoneTranslation(transform.position.z, targetPosition.z)))
        {
            float newX = Mathf.LerpUnclamped(transform.position.x, targetPosition.x, translationSpeed);
            float newZ = Mathf.LerpUnclamped(transform.position.z, targetPosition.z, translationSpeed);

            Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
            spaceshipBody.MovePosition(newPosition);
            return false;
        }

        return true;
    }

    private bool RotateTowardsPlayer()
    {
        Vector3 playerDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 relativePos = playerDirection - transform.position;

        Quaternion rotationRequired = Quaternion.LookRotation(relativePos, Vector3.up);

        var newRotation = Quaternion.LerpUnclamped(transform.rotation, rotationRequired, rotationSpeed);
        spaceshipBody.MoveRotation(newRotation);

        return true;
    }

    private bool CheckAlmostDoneTranslation(float f1, float f2)
    {
        return (Math.Abs(f1 - f2) < 0.5);
    }
}
