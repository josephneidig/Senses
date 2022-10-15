using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float dmgTimer;
    public float dmgTrigger;
    public Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
        Debug.Log(GameManager.gameManager._playerHealth.Health);
    }

    private void PlayerHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        Debug.Log(GameManager.gameManager._playerHealth.Health);
    }

    // I ADDED THESE
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            //PlayerTakeDmg(10);
            health.DamagePlayer(10);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            if (dmgTimer < dmgTrigger)
            {
                dmgTimer += Time.deltaTime;
            }
            else
            {
                health.DamagePlayer(10);
                dmgTimer = 0;
            }
        }
    }
}
