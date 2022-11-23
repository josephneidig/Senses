using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] private Animator myDoor = null; 
    [SerializeField] private bool openTrigger = false;
    [Serializerield] private bool closeTrigger = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.Play("doorAnimation", 0, 0.0f);
                gameObject.Setactive(false);
            }
            else if (closeTrigger)
            {
                 myDoor.Play("doorAnimation", 0, 0.0f);
                gameObject.Setactive(false);
            }
        }
    }
}
