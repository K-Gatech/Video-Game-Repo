using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeScene : MonoBehaviour
{
    public void LoadNewScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }
}
