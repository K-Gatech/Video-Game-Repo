using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertChestTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    GameObject chest;

    // Start is called before the first frame update
    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        chest = GameObject.FindGameObjectWithTag("ChestHouse");
    }

    // Update is called once per frame
    void Update()
    {
        if (chest.activeSelf && SaveGameDesert.GetCurrentState() >= SaveGameDesert.GameTransitionState.FoundTreasure1)
        {
            chest.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && chest.activeSelf)
        {
            chest.SetActive(false);
            SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.FoundTreasure1);

            string[] dialogues =
            {
                "Player's Thoughts",
                "NICE! This is quite a lot of gold. Let me trade this with The Collector for fuel, and get out of here for good.",
            };

            for (int i = 0; i < dialogues.Length; i += 2)
            {
                ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
            }

            ds.Show();
        }
    }
}
