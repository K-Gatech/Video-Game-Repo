using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomDialogues : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string[] dialogues;
    [SerializeField] GameObject playerGameObject;
    private DialogueSystem ds;
    private Animator anim;
    private bool isTalking;
    private bool thinking;

    // Start is called before the first frame update
    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
        anim = GetComponent<Animator>();
        playerGameObject.GetComponent<CharacterController>().enabled = false;
        playerGameObject.GetComponent<Animator>().enabled = false;
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTalking)
        {
            isTalking = true;
            dialogues = new string[2];
            dialogues[0] = "Player's Thoughts";
            dialogues[1] = "Damn! I should not have attacked those people. Where have they sent me???";

            for (int i = 0; i < dialogues.Length; i += 2)
            {
                ds.AddDialog(dialogues[i], dialogues[i + 1], 1);
            }
            ds.Show();
        }

        if (isTalking)
        {
            if (!ds.IsRunning())
            {
                isTalking = false;
                playerGameObject.GetComponent<CharacterController>().enabled = true;
                playerGameObject.GetComponent<Animator>().enabled = true;
                playerGameObject.GetComponent<Player>().enabled = true;
                Destroy(GameObject.Find("DialogueObj"));
            }
        }
    }
}
