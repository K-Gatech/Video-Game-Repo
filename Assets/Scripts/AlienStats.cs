using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlienStats : MonoBehaviour
{
    public GameObject alienObject;
    public float alienHealth;
    public float alienShield;
    public float alienDamage;
    public Slider healthSlider;

    private float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        alienObject.SetActive(true);
        alienHealth = alienHealth;    // 10 for sentry, 50 for guardian, 75 for stonemonster
        alienShield = alienShield;   // 0 for sentry, 0.25 for guardian, 0.5 for stonemonster
        alienDamage = alienDamage;   // 1 for sentry, 2 for guardian, 2 for stonemonster

        maxHealth = alienHealth;
    }

    void Update()
    {
        healthSlider.value = alienHealth / maxHealth;
        if(alienHealth <= 0)
        {
            alienObject.SetActive(false);
            if (alienObject.name.Equals("StoneMonster"))
            {
                PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("GreenPlanet");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            alienHealth -= (1 - alienShield);
            Debug.Log(alienHealth);
        }
    }
}
