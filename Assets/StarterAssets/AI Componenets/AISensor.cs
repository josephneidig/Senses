using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    // Which Sensor Type is it?
    public bool Seer;
    public bool Hearer;
    public bool Smeller;

    // Extra Smelling Variables
    public bool grow = false;
    private float timer;
    public float rangeGrowthRate = 1.001f;
    public float distanceReset = 5;

    // Base parameters
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> Objects
    {
        get
        {
            objects.RemoveAll(obj => !obj);
            return objects;
        }
    }
    private List<GameObject> objects = new List<GameObject>();


    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    // Temp values
    //private AudioManager AM;


    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;

        if (Smeller)
        {
            distance = distanceReset;
            grow = true;
            timer = 0.0f;
        }

        //Debug.Log(AudioManager.audioManager.exists());
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }

        if (Hearer)
        {
            if (AudioManager.audioManager.isPlaying())
            {
                //Debug.Log("Heard.");
                Objects.Clear();
                Objects.Add(GameObject.Find("PlayerCapsule"));
            }
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        Objects.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                Objects.Add(obj);
            }
            // For Hearer Only
            if (Hearer && AudioManager.audioManager.isPlaying())
            {
                Debug.Log("Found Player.");
                //Objects.Add(GameObject.Find("PlayerCapsule").GetComponenet<CapsuleCollider>());
            }
        }

        // For Smeller Only
        if (Smeller)
        {
            timer += Time.deltaTime;
            if (grow)
                distance += rangeGrowthRate;
        }
    }

    public bool CanHear()
    {
        if (AudioManager.audioManager.isPlaying())
        {
            Debug.Log("Heard.");
            return true;
        }
        return false;
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            // bottomm
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }


        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 1.0f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 1.0f);
        }
    }
}
