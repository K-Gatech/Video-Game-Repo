using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlienTalkController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] dialogues;
    [SerializeField] string[] alternateDialogue;
    private DialogueSystem ds;
    private TextMeshPro aiTalkPrompt;
    private GameObject player;
    private AlienLeaderTalkController leaderStatus;
    private bool firstDialogue;
    private bool inTalkRange;
    private bool isTalking;

    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        leaderStatus = GameObject.Find("AlienLeader").GetComponent<AlienLeaderTalkController>();
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
            if (leaderStatus.spokenWithLeader())
            {
                bool plant = true;
                for (int i = 0; i < dialogues.Length; i += 2)
                {
                    ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                    if (alternateDialogue[i + 1].Contains("Special plants")) {
                      plant = true;
                    }
                }
                
                if (plant == true) {
                  ds.AddObjective("Find the special plants that are facing down and Follow its direction");
                } 
                
                ds.Show();
            }
            else
            {
                
                for (int i = 0; i < alternateDialogue.Length; i += 2)
                {
                    ds.AddDialog(alternateDialogue[i], alternateDialogue[i + 1], 1);
                    
                }
                
                ds.AddObjective("Find the leader");
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

}
