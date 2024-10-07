using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InformationalPanel : MonoBehaviour
{
	GameObject player;
	
	public Text positionText;
	public Text altitudeText;
	
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
	
    void FixedUpdate()
    {
        positionText.text = "Position: " + player.transform.position.x + " " + player.transform.position.z;
		altitudeText.text = "Altitude: " + player.transform.position.y;
    }
}
