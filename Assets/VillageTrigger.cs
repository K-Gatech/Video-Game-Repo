using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    bool dialogRunning = false;
    bool seen = false;
    GameObject player = null;
  
    // Start is called before the first frame update
    void Start()
    {
        ds = GameObject.Find("Dialogue Canvas").GetComponent<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ds.IsRunning() && dialogRunning) {
          dialogRunning = false;
          
          player.GetComponent<CharacterController>().enabled = true;
          player.GetComponent<Animator>().enabled = true;
          player.GetComponent<CharacterController>().enabled = true;
          
          Destroy(this);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (seen) {
          return;
        }
      
        if (other.tag == "Player") {
          player = other.gameObject;
          player.GetComponent<CharacterController>().enabled = false;
          player.GetComponent<Animator>().enabled = false;
          player.GetComponent<CharacterController>().enabled = false;
          
          ds.AddDialog("Player", "Oh no, it has another species", 1)
            .AddDialog("Player", "Hopefully they are kind enough to have us here together, the sun is getting bigger and bigger", 1)
            .AddDialog("Player", "We need to find a place to stay as soon as possible", 1)
            .AddObjective("Talk to the alien")
            .Show();
          dialogRunning = true;
          seen = true;
        }
    }
}
