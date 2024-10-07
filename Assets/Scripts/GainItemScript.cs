using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GainItemScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] dialogues;
    [SerializeField] string[] alternateDialogue;
    [SerializeField] GameObject item;
    private DialogueSystem ds;
    private TextMeshPro aiTalkPrompt;
    private GameObject player;
    public GameObject boss;
    private bool firstDialogue;
    private bool inTalkRange;
    private bool isTalking;

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
        if(true)//!boss.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E) && inTalkRange && !isTalking)
            {
                Debug.Log("talk");
                isTalking = true;
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<Animator>().enabled = false;
                if (firstDialogue)
                {
                    for (int i = 0; i < dialogues.Length; i += 2)
                    {
                        ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                    }
                    ds.AddObjective("Go to the Guardian Forest");
                    ds.Show();
                    firstDialogue = !firstDialogue;
                }
            }
            if (isTalking)
            {
                if (!ds.IsRunning())
                {
                    isTalking = false;
                    player.GetComponent<CharacterController>().enabled = true;
                    player.GetComponent<Animator>().enabled = true;

                    // Gain item to appease guardian
                    Player p = player.GetComponent<Player>();
                    if (item.CompareTag("GuardianDesire"))
                    {
                        p.GetGuardianDesire();
                    }
                    else if (item.CompareTag("Treasure"))
                    {
                        p.GetRepairKit();
                    }
                    // Destroy spot
                    Destroy(item);
                    player.GetComponent<CharacterController>().enabled = true;
                    player.GetComponent<Animator>().enabled = true;
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
