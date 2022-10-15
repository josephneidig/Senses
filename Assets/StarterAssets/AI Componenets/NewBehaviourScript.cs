using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;

    public Vector3 destination;
    bool destSet;
    public float destRange;

    public float viewDist;
    bool inViewDist;
    bool canSee;

    //public AISensor lineOfSight;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        //lineOfSight = GetComponent<AISensor>();
    }

    private void Update()
    {
        // I'm going insane. Calling LineOfSight.Objects causes the error 
        // "Object reference not set to an instance of an object"
        //Debug.Log(lineOfSight.Objects.Count);
        canSee = true;
        //inViewDist = Physics.CheckSphere(transform.position, viewDist, isPlayer);

        if (!canSee) 
            Patrol();
        else 
            Follow();
    }

    private void Patrol()
    {
        if (!destSet)
            FindNextDest();
        else
            agent.SetDestination(destination);

        Vector3 distanceToDest = transform.position - destination;

        if (distanceToDest.magnitude < 1f)
            destSet = false;
    }

    private void FindNextDest()
    {
        NavMeshPath path = new NavMeshPath();

        float randZ = Random.Range(-destRange, destRange);
        float randX = Random.Range(-destRange, destRange);

        float newX = transform.position.x + randX;
        float newY = transform.position.y;
        float newZ = transform.position.z + randZ;

        destination = new Vector3(newX, newY, newZ);

        if (agent.CalculatePath(destination, path))
            destSet = true;
        else
            FindNextDest();
    }

    private void Follow()
    {
        agent.SetDestination(player.position);
    }
}
