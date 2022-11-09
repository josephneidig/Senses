using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float dmgTimer;
    public float dmgTrigger;
    public Health health;

    public float lasthit;
    public float timer;
    //public Animator enemy_Animator;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        timer = 0;
        lasthit = -10;
        //enemy_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
        if (collision.collider.gameObject.CompareTag("Enemy") &&
            timer - lasthit > 2)
        {
            //enemy_Animator.SetBool("collideWithPlayer", true);
            //PlayerTakeDmg(10);
            health.DamagePlayer(10);
            lasthit = timer;
        }
    }

/*    private void OnCollisionStay(Collision collision)
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
    }*/
}
