using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPlantTrigger : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider c)
    {
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
}
