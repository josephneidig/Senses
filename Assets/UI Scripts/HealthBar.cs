using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Health playerHealth;
	public GameObject DeathScreen;

    private void Start()
    {
		Hide();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = playerHealth.maxHealth;
    }
	
	public void Died ()
	{
		DeathScreen.SetActive(true);
	}
	public void Hide()
	{
		DeathScreen.SetActive(false);
	}

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
		if(hp <= 0)
			Died();
    }
}
