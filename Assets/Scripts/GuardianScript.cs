using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuardianScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string[] dialogues;
    [SerializeField] string[] alternateDialogue;
    [SerializeField] Player player;
    [SerializeField] GameObject playerGameObject;
    private DialogueSystem ds;
    private Animator anim;
    private bool isTalking, aggressiveDialogComplete;
    private bool needToTeleport;
    private BossAI bossAI;

    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        anim = GetComponent<Animator>();
        isTalking = false;
        aggressiveDialogComplete = false;
        bossAI = this.GetComponent<BossAI>();
    }

    private void Update()
    {
        if (isTalking)
        {
            if (!ds.IsRunning())
            {
                isTalking = false;
                if (needToTeleport)
                {
                    playerGameObject.transform.position = player.teleport.position;
                    needToTeleport = false;
                }
                else
                {
                    GameObject guardian = GameObject.Find("Crocodile_LOD1");
                    Destroy(guardian);
                }
                playerGameObject.GetComponent<CharacterController>().enabled = true;
                playerGameObject.GetComponent<Animator>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(bossAI.bossState == BossAI.BossState.Passive)
        {
            if (other.gameObject.CompareTag("Player") && !isTalking)
            {

                playerGameObject.GetComponent<CharacterController>().enabled = false;
                playerGameObject.GetComponent<Animator>().enabled = false;
                if (player.HasGuardianDesire())
                {
                    
                    for (int i = 0; i < dialogues.Length; i += 2)
                    {
                        ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
                    }
                    ds.Show();

                }
                else
                {
                    needToTeleport = true;
                    for (int i = 0; i < alternateDialogue.Length; i += 2)
                    {
                        ds.AddDialog(alternateDialogue[i], alternateDialogue[i + 1], 1);
                    }
                    ds.Show();
                }
                isTalking = true;
            }
        }
        else
        {
            if(!aggressiveDialogComplete)
            {
                // agressive dialog
                aggressiveDialogComplete = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        return;
    }

    IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
