using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour, IDamageable
{
    private float health = 100f;
    public AudioSource hit;

    public void Damage(float damage)
    {
        hit.Play();
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
