using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;
    //public GameObject self;

    public Vector3 destination;
    bool destSet;
    public float destRange;

    public float radius = 2.5f;
    public float cirDist = 3.5f;
    public float selectTimer = 0f;
    public float selectInterval = 0.1f;

    public float viewDist;
    bool inViewDist;
    bool canSee;

    private Vector3 velocity;
    private Vector3 wanderForce;

    //private bool stuck = false;
    public float stuckMagnitude = 0.01f;
    public float stuckCheck = 2f;
    private float stuckTimer;

    public bool debug;

    //private bool following = false;

    public AISensor lineOfSight;
    public Animator enemy_Animator;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        lineOfSight = GetComponent<AISensor>();
        enemy_Animator = GetComponent<Animator>();

        enemy_Animator.SetBool("isMoving", true);
        enemy_Animator.SetBool("collideWithPlayer", false);
        enemy_Animator.SetBool("isDead", false);

        stuckTimer = stuckCheck;

    }

    private void Update()
    {
        // I'm going insane. Calling LineOfSight.Objects causes the error 
        // "Object reference not set to an instance of an object"
        //Debug.Log(lineOfSight.Objects.Count);
        canSee = lineOfSight.Objects.Count > 0;
        //inViewDist = Physics.CheckSphere(transform.position, viewDist, isPlayer);

        if (!canSee)
        {
            Patrol();
        }
        else
        { 
            Follow();
        }
    }

    private void Patrol()
    {
        /*if (agent.velocity.magnitude < stuckMagnitude)
        {
            Debug.Log("Got Stuck, picking new destination...");
            stuck = true;
        }
        else
        {
            stuck = false;
        }*/

        /*selectTimer -= Time.deltaTime;
        if (selectTimer < 0)
        {
            selectTimer += selectInterval;
            UpdatedFindNextDest();
        }

        agent.SetDestination(destination);*/

/*        if (debug)
        {
            Debug.Log(agent.velocity.magnitude);
        }

        if (agent.velocity.magnitude < stuckMagnitude)
        {
            stuckTimer -= Time.deltaTime;
        }
        else
        {
            stuckTimer = stuckCheck;
        }

        if (stuckTimer < 0)
        {
            stuckTimer = stuckCheck;
            PickDestBehind();
        }*/


        if (!destSet)
            UpdatedFindNextDest();
        else
            agent.SetDestination(destination);

        Vector3 distanceToDest = transform.position - destination;

        if (debug)
            Debug.Log("Velocity is " + agent.velocity.magnitude);

        if (distanceToDest.magnitude <= 1f)
            destSet = false;

        if (agent.velocity.magnitude < 0.75)
        {
            stuckTimer -= Time.deltaTime;
        }
        else
        {
            stuckTimer = stuckCheck;
        }

        if (stuckTimer < 0)
        {
            stuckTimer = stuckCheck;
            destSet = false;
        }



    }

    private void ScootBack()
    {
        NavMeshPath path = new NavMeshPath();

        float newY = transform.position.y;
        float newZ = transform.position.z + (transform.forward.z * -2);
        float newX = transform.position.x;

        destination = new Vector3(newX, newY, newZ);

        if (agent.CalculatePath(destination, path))
            destSet = true;
        else
            UpdatedFindNextDest();
    }

    private void TurnAround()
    {
        transform.RotateAround(transform.position, Vector3.up, 45 * Time.deltaTime);
    }

    /* 
    private void Go()
    {
        NavMeshPath path = new NavMeshPath();

        float randX = Random.Range(-destRange, destRange);

        float newY = transform.forward.y;
        float newX = transform.forward.z + 2;
        float newZ = transform.forward.x + randX;

        destination = new Vector3(newX, newY, newZ);

        if (agent.CalculatePath(destination, path))
            destSet = true;
        else
            Go();
    }*/


    private void FindNextDest()
    {
        if (debug)
            Debug.Log("Random choice");
        NavMeshPath path = new NavMeshPath();

        float randZ = Random.Range(-destRange, destRange);
        float randX = Random.Range(-destRange, destRange);

        /*
        float newX = transform.forward.x + randX;
        float newY = transform.position.y;
        float newZ = transform.forward.z + randZ;
        */
        
        float newX = transform.position.x + randX;
        float newY = transform.position.y;
        float newZ = transform.position.z + randZ;
        

        destination = new Vector3(newX, newY, newZ);

        // ==== NEW ==== //
        NavMeshHit hit;
        NavMesh.FindClosestEdge(destination, out hit, NavMesh.AllAreas);

        if (debug)
            Debug.Log((hit.position - destination).magnitude);

        float distanceCheck = (hit.position - destination).magnitude;

        if (distanceCheck < 0.5f || distanceCheck > 10000f)
        {
            if (debug)
                Debug.Log("too close to wall");
            destSet = false;
            FindNextDest();
            return;
        }

        if (agent.CalculatePath(destination, path))
            destSet = true;
        else
            FindNextDest();
    }

    private void UpdatedFindNextDest()
    {
        if (debug)
            Debug.Log("Circle choice");
        NavMeshPath path = new NavMeshPath();

        // create a circle with radius r and distance x in front of the enemy
        float randTheta = Random.Range(0, 2 * (Mathf.PI));
        //Debug.Log("Random Angle is :" + randTheta);

 /*       float cD = cirDist;
        if (stuck)
            cD = -1 * cirDist;*/

        float newY = transform.position.y + transform.forward.y;
        float newX = (transform.position.x + transform.forward.x) + (radius * square(Mathf.Cos(randTheta)));
        float newZ = (transform.position.z + (cirDist * transform.forward.z)) + (radius * square(Mathf.Sin(randTheta)));
        //Debug.Log("New X is " + (cirDist * self.transform.forward.x) + " + " + (radius * square(Mathf.Cos(randTheta))));
        //Debug.Log("New Z is " + (cirDist * self.transform.forward.z) + " + " + (radius * square(Mathf.Sin(randTheta))));

        destination = new Vector3(newX, newY, newZ);

        // ==== NEW ==== //
        NavMeshHit hit;
        NavMesh.FindClosestEdge(destination, out hit, NavMesh.AllAreas);

        if (debug)
            Debug.Log((hit.position - destination).magnitude);

        float distanceCheck = (hit.position - destination).magnitude;

        if (distanceCheck < 0.5f || distanceCheck > 10000f)
        {
            if (debug)
                Debug.Log("too close to wall, random point now.");
            destSet = false;
            FindNextDest();
            return;
        }

        // path to that destination using navmesh
        if (agent.CalculatePath(destination, path))
            destSet = true;
        else
            UpdatedFindNextDest();
    }

    private void PickDestBehind()
    {
        NavMeshPath path = new NavMeshPath();

        float randTheta = Random.Range(0, Mathf.PI);

        float newY = transform.position.y + transform.forward.y;
        float newX = (transform.position.x + transform.forward.x) + (radius * square(Mathf.Cos(randTheta)));
        float newZ = (transform.position.z + (-cirDist * transform.forward.z)) + (radius * square(Mathf.Sin(randTheta)));
        //Debug.Log("New X is " + (cirDist * self.transform.forward.x) + " + " + (radius * square(Mathf.Cos(randTheta))));
        //Debug.Log("New Z is " + (cirDist * self.transform.forward.z) + " + " + (radius * square(Mathf.Sin(randTheta))));

        destination = new Vector3(newX, newY, newZ);

        // path to that destination using navmesh
        if (agent.CalculatePath(destination, path))
            destSet = true;
        else
            PickDestBehind();
    }

    private float square(float a)
    {
        return (a * Mathf.Abs(a));
    }

    private void Follow()
    {
        agent.SetDestination(player.position);
        destSet = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision with player initiated.");
            enemy_Animator.SetBool("collideWithPlayer", true);
            enemy_Animator.SetBool("isMoving", false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        enemy_Animator.SetBool("isMoving", true);
        enemy_Animator.SetBool("collideWithPlayer", false);

        //FindNextDest();
    }

    
}
