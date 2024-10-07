using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GreenPlanetStartingDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] dialogues;
    [SerializeField] GameObject playerGameObject;
    private DialogueSystem ds;
    private Animator anim;
    private bool isTalking;
    private bool thinking;

    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        anim = GetComponent<Animator>();
        playerGameObject.GetComponent<CharacterController>().enabled = false;
        playerGameObject.GetComponent<Animator>().enabled = false;
        isTalking = false;
    }

    private void Update()
    {
        if (!isTalking)
        {
            isTalking = true;

            var pref = PlayerPrefs.GetString("PreviousScene");
            if (pref.Equals("DoomPlanet"))
            {
                dialogues = new string[10];
                dialogues[0] = "Player's Thoughts";
                dialogues[1] = "Phew! That was terrifying. Shouldn't try to harm these people again.";
                dialogues[2] = "Player's Thoughts";
                dialogues[3] = "Wait...what people? I feel like I just remembered something, but it all feels so hazy.";
                dialogues[4] = "Player's Thoughts";
                dialogues[5] = "Wait... Huh? readings indicate I have gone back in time to when I first landed on the planet!";
                dialogues[6] = "Player's Thoughts";
                dialogues[7] = "Talk about a sense of deja vu.";
                dialogues[8] = "Player's Thoughts";
                dialogues[9] = "Not sure what happened, but I feel like I know a bit more of what I need to do.";
                PlayerPrefs.DeleteAll();

                for (int i = 0; i < dialogues.Length; i += 2)
                {
                    ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                }
                ds.AddObjective("Walk around");
                ds.Show();
            }
            else if (pref.Equals("GreenPlanet"))
            {
                dialogues = new string[8];
                dialogues[0] = "Player's Thoughts";
                dialogues[1] = "I'm dea.... Huh?";
                dialogues[2] = "Player's Thoughts";
                dialogues[3] = "... Readings indicate I have gone back in time to when I first landed on the planet!";
                dialogues[4] = "Player's Thoughts";
                dialogues[5] = "Talk about a sense of deja vu.";
                dialogues[6] = "Player's Thoughts";
                dialogues[7] = "Guess I'll try something different this time round.";
                PlayerPrefs.DeleteAll();

                for (int i = 0; i < dialogues.Length; i += 2)
                {
                    ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                }
                ds.AddObjective("Walk around");
                ds.Show();
            }
            else
            {
                for (int i = 0; i < dialogues.Length; i += 2)
                {
                    ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                }
                ds.AddObjective("Walk around");
                ds.Show();
            }
        }
        if (isTalking)
        {
            if (!ds.IsRunning())
            {
                isTalking = false;
                playerGameObject.GetComponent<CharacterController>().enabled = true;
                playerGameObject.GetComponent<Animator>().enabled = true;
                playerGameObject.GetComponent<Player>().enabled = true;
                Destroy(GameObject.Find("StartingDialogue"));
            }
        }
    }
}