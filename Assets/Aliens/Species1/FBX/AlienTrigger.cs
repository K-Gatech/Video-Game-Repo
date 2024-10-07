using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlienTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    private Animator anim;
    private bool IsTalking;
    private GameObject player;
    private GameObject lavaChest;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        lavaChest = GameObject.FindGameObjectWithTag("ChestLava");
        anim = GetComponent<Animator>();
        IsTalking = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "DesertPlanet")
        {
            return;
        }

        if (!IsTalking && SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.FoundTreasure1 && Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.CameToAlienAgain);
        }

        if (IsTalking && SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.Started)
        {
            if (ds.IsSequenceDone())
            {
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<Animator>().enabled = true;
                SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.TalkedToAlien);
                IsTalking = false;
            }
        }

        if (IsTalking && SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.CameToAlienAgain)
        {
            if (ds.IsSequenceDone())
            {
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<Animator>().enabled = true;
                SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.TalkedToAlienAgain);
                IsTalking = false;
            }
        }

        if (!IsTalking && SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.FoundTreasure2 && Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.CameToAlienYetAgain);
        }

        if (IsTalking && SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.CameToAlienYetAgain)
        {
            if (ds.IsSequenceDone())
            {
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<Animator>().enabled = true;
                SaveGameDesert.TransitionToState(SaveGameDesert.GameTransitionState.PaidToAlien);
                IsTalking = false;
            }
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (SceneManager.GetActiveScene().name != "DesertPlanet" || !player.GetComponent<Player>().isGrounded)
        {
            return;
        }

        if (SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.Started && !IsTalking)
        {
            StartDialogue_1(c.gameObject);
            player = c.gameObject;
            IsTalking = true;
        }
        else if (SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.CameToAlienAgain && !IsTalking)
        {
            StartDialogue_2(c.gameObject);
            player = c.gameObject;
            IsTalking = true;
        }
        else if (SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.CameToAlienYetAgain && !IsTalking)
        {
            StartDialogue_3(c.gameObject);
            player = c.gameObject;
            IsTalking = true;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        Debug.Log("Player entered");
        if (c.gameObject.CompareTag("Player"))
        {
            anim.SetBool("PlayerClose", true);
        }
    }
    private void OnTriggerExit(Collider c)
    {
        Debug.Log("Player exited");
        anim.SetBool("PlayerClose", false);
    }

    private void StartDialogue_1(GameObject player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;

        string[] dialogues =
        {
            "Alien",
            "Hey there, stranger! Do you find yourself lost on this desolate wasteland?",
            "Player",
            "Indeed. We ran out of fuel, had to make an unexpected landing here.",
            "Alien",
            "Oh, tragic... the expanding Sun has already claimed all life here.",
            "Player",
            "What are you doing here? And do you happen to have any fuel?",  /* https://marvelcinematicuniverse.fandom.com/wiki/Collector */
            "Alien",
            "I am The Collector. I roam the Multiverse in search of treasures.",
            "Alien",
            "I do not require fuel. Surely I can give you some from my collections. But nothing comes free.",
            "Alien",
            "I have heard much about the treacherous maze on this planet, and the treasures that behold anyone who dares enter...",
            "Player",
            "Hmm.. okay, let me give it a try. What choice do I have.",
            "Alien",
            "Go on, it's over there, behind me."
        };

        for (int i = 0; i < dialogues.Length; i+=2)
        {
            ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
        }

        ds.Show();
    }

    private void StartDialogue_2(GameObject player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;

        string[] dialogues =
        {
            "Player",
            "Hey there! I'm back with some gold.",
            "Alien",
            "Oh, really? You managed to escape the maze alien. That is some achievement.",
            "Player",
            "I found this key in the maze, and then went looking for a treasure to open with it.",
            "Alien",
            "This tiny box, you call this a treasure. No, no, this will not do. Life ain't so easy my friend.",
            "Alien",
            "But I can think about giving you what you need, if you get me some more of this. I remember seeing something on top of those wooden stacks, THERE, over the lava.",
            "Player",
            "Oh come on. Are you serious? You claim to be this all-powerful Collector, but you need my help to get to that?",
            "Alien",
            "Let's just say, I haven't had any entertainment for a while.",
            "Player",
            "Hmm.. okay, let me give it a try. What choice do I have."
        };

        for (int i = 0; i < dialogues.Length; i += 2)
        {
            ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
        }

        player.transform.LookAt(lavaChest.transform);
        ds.Show();
    }

    private void StartDialogue_3(GameObject player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;

        string[] dialogues =
        {
            "Alien",
            "I saw what you just did. You are a daredevil...",
            "Player",
            "Enough of your sadistic games. Now give me the fuel you promised.",
            "Alien",
            "Alright, alright. Here it is.",
            "Player",
            "Thanks a lot. My people and I can finally be on our way to a new home.",
            "Alien",
            "Good luck on your journey, brave soul.",
        };

        for (int i = 0; i < dialogues.Length; i += 2)
        {
            ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
        }

        ds.Show();
    }
}
