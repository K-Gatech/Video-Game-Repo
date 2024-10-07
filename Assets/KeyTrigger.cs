using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    GameObject key;

    // Start is called before the first frame update
    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        key = GameObject.FindGameObjectWithTag("KeyDesert");
    }

    // Update is called once per frame
    void Update()
    {
        if (key.activeSelf && SaveGameDesert.GetCurrentState() > SaveGameDesert.GameTransitionState.ReachedMaze)
        {
            key.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && key.activeSelf)
        {
            key.SetActive(false);
            SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.FinishedMaze);

            string[] dialogues =
            {
                "Player's Thoughts",
                "Looks like the treasure that Collector guy spoke of. Or rather, a key to a treasure. I probably need to find what this unlocks. I did see a building that is different from all others. That could be a place to look.",
            };

            for (int i = 0; i < dialogues.Length; i+=2)
            {
                ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
            }

            ds.Show();
        }
    }
}
