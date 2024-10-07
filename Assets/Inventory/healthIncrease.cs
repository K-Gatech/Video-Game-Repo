using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthIncrease : MonoBehaviour
{

    public GameObject item;
    Player player;
    // Start is called before the first frame update
    void Awake()
    {
        //HealthIncrease();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    public void onClick()
    {
        
        player.IncreaseHealth();
        Destroy(gameObject);
        
    }
}
