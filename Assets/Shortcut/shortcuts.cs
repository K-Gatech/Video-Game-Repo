using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shortcuts : MonoBehaviour
{
    public GameObject shortcut;
    public GameObject inventory;

    public static bool isGamePaused = false;

    //[SerializeField] GameObject Shortcuts;
  
    private void Start()
    {
        shortcut.gameObject.SetActive(false);
        //inventory.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //shortcut.gameObject.SetActive(!shortcut.gameObject.activeSelf);
            
            if (isGamePaused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
            
        }

        void ResumeGame()
        {
            //shortcut.gameObject.SetActive(!shortcut.gameObject.activeSelf);
            shortcut.gameObject.SetActive(false);
            Time.timeScale = 1f;
            isGamePaused = false;
        }

        void PauseGame()
        {
            shortcut.gameObject.SetActive(!shortcut.gameObject.activeSelf);
            Time.timeScale = 0f;
            isGamePaused = true;
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
           
            inventory.gameObject.SetActive(!inventory.gameObject.activeSelf);
        }
    }
}
