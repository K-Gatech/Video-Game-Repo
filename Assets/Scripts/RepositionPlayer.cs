using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;

    void Start()
    {
        player.transform.position = new Vector3(200f, 2.6f, -203f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
