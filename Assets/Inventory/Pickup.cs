using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    private Inventory inventory;
    public GameObject itemButton;
    public GameObject effect;


    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();     

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
                       

            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false) {                   
                    inventory.isFull[i] = true;                 
                    Instantiate(itemButton, inventory.slots[i].transform.position, inventory.slots[i].transform.rotation, inventory.slots[i].transform);
                    Destroy(gameObject);
                    
                    break;
                }
            }
        }
        
    }
}
