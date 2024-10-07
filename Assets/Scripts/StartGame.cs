using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartTheGame()
    {
        SceneManager.LoadScene("DesertPlanet");
        Time.timeScale = 1f;
    }
}
