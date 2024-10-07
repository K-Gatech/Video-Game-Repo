using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
        string SavePath = Path.Combine(Application.persistentDataPath, "savedata.txt");
      }
    }
    
    public void GameOver() {
      GameObject.Find("GameOver Canvas").GetComponent<Animator>().SetTrigger("GameOver");
    }
}
