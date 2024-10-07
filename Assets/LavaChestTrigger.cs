using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaChestTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    GameObject chest;

    // Start is called before the first frame update
    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        chest = GameObject.FindGameObjectWithTag("ChestLava");
    }

    // Update is called once per frame
    void Update()
    {
        if (chest.transform.position.y < 0 && (SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.CameToAlienAgain || SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.TalkedToAlienAgain))
        {
            chest.transform.position = new Vector3(chest.transform.position.x, 19, chest.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && chest.activeSelf)
        {
            SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.FoundTreasure2);
            chest.SetActive(false);
        }
    }
}
