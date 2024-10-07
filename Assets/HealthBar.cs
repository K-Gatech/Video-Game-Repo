using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	
	public Image[] image;
	public float maxHealth = 100;
	
	private float currentHealth;
	
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        int percentageHealth = (int)Mathf.Ceil((currentHealth / maxHealth) * 10);
		
		for (int i = 0; i < 10; i++) {
			image[i].enabled = (i < percentageHealth);
		}
    }
	
	public void AddDamage(float damage) {
		currentHealth -= damage;
		currentHealth = Mathf.Max(currentHealth, 0);
	}
	
	public void SetHealth(float health) {
		currentHealth = health;
	}
	
	public void AddHealth(float health) {
		currentHealth += health;
		currentHealth = Mathf.Min(currentHealth, maxHealth);
	}
}
