using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public GameObject potion;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //potion.SetActive(false);
            transform.GetChild(1).GetComponent<PlayerHealthController>().ChangeHealth(3.0f);
        }
    }
}
