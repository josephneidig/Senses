using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorscript : MonoBehaviour
{
    Animator animator;
    bool doorOpen;

    void Start()
    {
        Debug.Log("starting");


        doorOpen = false;
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" | col.gameObject.tag == "Enemy")
        {

            doorOpen = true;
            Doors("open");
        }
        
    }

     void OnTriggerExit(Collider col)
    {
        if(doorOpen)
        {
            doorOpen = false;
            Doors("close");
        }
    }

    void Doors(string direction)
    {
        animator.SetTrigger(direction);
    }
}
