using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    HealthStatus healthstate;
    // Image healthBar;
	
	private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthstate = new HealthStatus(SaveGameDesert.GetSavedPlayerHealth());
        // healthBar = GameObject.FindGameObjectWithTag("HealthMissing").GetComponent<Image>();
		healthBar = GameObject.Find("Health Bar Panel").GetComponent<HealthBar>();

        if (healthstate.currentLevel <= 0.1)
        {
            healthstate.currentLevel = 100;
            SaveGameDesert.SaveHealth();
        }
        UpdateHealthBar(healthstate.currentLevel);
    }

    public float GetHealthLevel()
    {
        return healthstate.currentLevel;
    }

    public void UpdateHealthBar(float newHealth)
    {
        // healthBar.fillAmount = Mathf.Clamp(newHealth / 100, 0, 1f);
		healthBar.SetHealth(Mathf.Clamp(newHealth, 0, 100));
		// No implementation!
    }

    public void ChangeHealth(float changePercent)
    {
        if (healthstate.currentLevel + changePercent < 0)
        {
            changePercent = healthstate.currentLevel;
        }
        else if (healthstate.currentLevel + changePercent > 100)
        {
            changePercent = (100 - healthstate.currentLevel);
        }

        healthstate.currentLevel += changePercent;
        UpdateHealthBar(healthstate.currentLevel);

        if (healthstate.currentLevel <= 0.1)
        {
            var sceneDiedOn = SceneManager.GetActiveScene().name;
            if (sceneDiedOn.Equals("GreenPlanet"))
            {
                PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
            }
            if (sceneDiedOn.Equals("DoomPlanet"))
            {
                PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("GreenPlanet");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
