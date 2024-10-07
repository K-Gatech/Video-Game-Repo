using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer5 : MonoBehaviour
{

    public int CountdownTime;
    public Transform Teleport;
    public GameObject players;

    public GameObject textDisplay;
    public bool takingAway = false;
    
    // Start is called before the first frame update
    void Start()
    {              
        textDisplay.GetComponent<Text>().text = "00:" + CountdownTime;
        //takingAway = false;
    }

    private void Update()
    {
        if (takingAway == false )
        {
            StartCoroutine(CountdownToStart());
        }
    }


    IEnumerator CountdownToStart()
    {

        if (CountdownTime == 0)
        {
            textDisplay.GetComponent<Text>().text = "You Lose";
            yield return new WaitForSecondsRealtime(5);
            textDisplay.GetComponent<Text>().text = " ";
            UnityEngine.SceneManagement.SceneManager.LoadScene("DesertPlanet");

            //players.transform.position = Teleport.transform.position;
            //yield return new WaitForSecondsRealtime(2);
            //textDisplay.GetComponent<Text>().text = " ";

        }

        else

        {

            takingAway = true;
            yield return new WaitForSeconds(1);

            CountdownTime -=1;
            textDisplay.GetComponent<Text>().text = CountdownTime + " seconds left";
        }
        

        takingAway = false;


    }
    
}
