using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlienLeaderTalkController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] dialogues;
    [SerializeField] string[] alternateDialogue;
    private DialogueSystem ds;
    private TextMeshPro aiTalkPrompt;
    private GameObject player;
    private bool firstDialogue;
    private bool inTalkRange;
    private bool isTalking;
    private bool endGame = false;

    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        aiTalkPrompt = GetComponentInChildren<TextMeshPro>();
        aiTalkPrompt.gameObject.SetActive(false);
        firstDialogue = true;
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inTalkRange && !isTalking)
        {
            Debug.Log("talk");
            isTalking = true;
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<Animator>().enabled = false;
            if (GameObject.Find("TreasureChestPrefab") == null) 
            {
                ds.AddDialog("Alien Leader", "The treasure do you have", 1)
                  .AddDialog("Player", "This is it. This is your legendary treasure", 1)
                  .AddDialog("Alien Leader", "You thank. Since a long time ago we try to find it", 1)
                  .AddDialog("Player", "So, can we stay?", 1)
                  .AddDialog("Alien Leader", "Think let me.", 1)
                  .Show();
                
                endGame = true;
            }
            if (firstDialogue)
            {
                for (int i = 0; i < dialogues.Length; i += 2)
                {
                    ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                }
                ds.AddObjective("Ask the villagers to find the way.");
                ds.Show();
                firstDialogue = !firstDialogue;
            }
            else
            {
                bool plant = false;
                for (int i = 0; i < alternateDialogue.Length; i += 2)
                {
                    ds.AddDialog(alternateDialogue[i], alternateDialogue[i + 1], 1);
                    if (alternateDialogue[i + 1].Contains("Special plants")) {
                       plant = true;
                    }
                }
                
                if (plant == true) {
                  ds.AddObjective("Follow the special plants.");
                }
                ds.Show();
            }
        }
        
        if (isTalking)
        {
            if (!ds.IsRunning())
            {
                isTalking = false;
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<Animator>().enabled = true;
                
                if (endGame) {
                  GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            aiTalkPrompt.gameObject.SetActive(true);
            inTalkRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        aiTalkPrompt.gameObject.SetActive(false);
        inTalkRange = false;
    }

    public bool spokenWithLeader()
    {
        return !firstDialogue;
    }
}
