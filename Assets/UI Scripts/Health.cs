using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image screenRed;
    [SerializeField] private AudioClip breathSFX;

    public int curHealth = 0;
    public int maxHealth = 100;

    private float timeSinceLastDamage;
    private float regenTime;
    private float timeSinceLastBreath;

    public HealthBar healthBar;

    void Start()
    {
        curHealth = maxHealth;
        regenTime = 0f;
        timeSinceLastDamage = 0f;
        timeSinceLastBreath = 0f;
    }

    void Update()
    {
        if (timeSinceLastDamage >= 6.5)
        {
            regenTime += Time.deltaTime;
            if (regenTime >= 0.1f)
            {
                curHealth = (int)Mathf.Min(curHealth + 1, (float)maxHealth);
                regenTime = 0f;
            }
            screenRed.color = new Color(1f, 0f, 0f, Mathf.Max(0f, (100f - (float)curHealth) / 250f));
        }

        if (curHealth <= 50)
        {
            if ((timeSinceLastBreath) > 5f + ((float)curHealth / 10f))
            {
                AudioManager.audioManager.PlaySound(breathSFX);
                timeSinceLastBreath = 0f;
            }
            else
            {
                timeSinceLastBreath += Time.deltaTime;
            }
        }
        else
        {
            timeSinceLastBreath = 0f;
        }

        timeSinceLastDamage += Time.deltaTime;
    }

    public void DamagePlayer( int damage )
    {
        curHealth -= damage;
        Debug.Log(curHealth);

        healthBar.SetHealth( curHealth );
        if (curHealth <= 0)
        {
            screenRed.color = new Color(1f, 0f, 0f, 0f);
        }
        else
        {
            screenRed.color = new Color(1f, 0f, 0f, Mathf.Max(0f, (100f - (float)curHealth) / 250f));
        }
        timeSinceLastDamage = 0f;
    }

    public void UpgradeHealth(int amount)
    {
        maxHealth += amount;
    }
}