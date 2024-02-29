using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Original Plane-mesh generation Code obtained from: https://gist.github.com/goldennoodles/38be017efae5459d34120550645c73f9

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
// Requires these components for the script to actually run, will throw an error otherwise.

public class Working_Gravitation_Manager : MonoBehaviour
{
     //Mesh Generation References
    Mesh myMesh;
    MeshFilter meshFilter;

    //Mesh Generation Plane Settings
    [SerializeField] Vector2 planeSize = new Vector2(1, 1);
    [SerializeField] int planeResolution = 1;

    //Mesh Generation Mesh Properties
    List<Vector3> vertices;
    List<int> triangles;

    public float G = 100f; //Gravitational constant
    public float w = 1.0f; //Omega (Angular velocity)

    public float k = 1.0f; //Potential Deformation Constant of porportionality
    GameObject[] celestialObjects; //Array containing all celestial objects in the scene
    // Start is called before the first frame update
    void Start()
    {
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        InitialVelocity();
        //Initialise Action Surface:
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;

    }

    void FixedUpdate()
    {
        //apply gravitational force to all relevant bodies
        compute_Gravity(celestialObjects);
        
        //update the Potential Surface:
        //planeResolution = Mathf.Clamp(planeResolution, 1, 1001);

        //GeneratePlane(planeSize, planeResolution);
        //Deform();
        //AssignMesh();
    }

    //gravity must be computed now as the law of gravitation + the deformation of the mesh at that point due to player input.
    //as such, the arguments is the celestial objects in the scene, and the 
    void compute_Gravity(GameObject[] celestialObjects)
    {
        //first part of the code computes the gravitational interactions.
        foreach (GameObject a in celestialObjects)
        {
            foreach (GameObject b in celestialObjects)
            {
                if(!a.Equals(b)) // handles the interaction of the body a with other bodies b in the scene
                {
                    //calculate force due to gravity from Newton's law of gravitation:
                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    //applies the calculated linear force and angular velocity to the necessary rigidbody, as well
                    a.GetComponent<Rigidbody>().AddForce((b.transform.position - a.transform.position).normalized * (G * (m1 * m2)/(r*r)));
                    a.GetComponent<Rigidbody>().angularVelocity += Vector3.up * w; //N.B. Arbitrary for now, FIX THIS!!!
                }

            }
        }
    }

    void InitialVelocity()
    {
        foreach (GameObject a in celestialObjects)
        {
            foreach (GameObject b in celestialObjects)
            {
                if(!a.Equals(b))
                {
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform);

                    a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G*m2)/r);
                    a.GetComponent<Rigidbody>().angularVelocity += Vector3.up * w;
                }
            }
        }
    }
    void GeneratePlane(Vector2 size, int resolution)
    {
        //Create vertices:
        vertices = new List<Vector3>();
        float xPerStep = size.x/resolution;
        float yPerStep = size.y/resolution;

        for (int y = 0; y<resolution+1; y++)
        {
            for (int x = 0; x<resolution+1; x++)
            {
                vertices.Add(new Vector3(x*xPerStep, 0, y*yPerStep));
            }
        }

        triangles = new List<int>();
        for (int row = 0; row<resolution; row++)
        {
            for (int column = 0; column <resolution; column++)
            {
                int i = (row*resolution) + row + column;
                
                //first triangle
                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);

                //second triangle
                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i+1);
            }
        }
    }
    void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
    }

    void Deform()
    {
        //iterates through all vertices
        for (int i = 0; i<vertices.Count; i++)
        {
            //selects the  current vertex and stores it as a variable
            Vector3 vertex = vertices[i];
            //performs some manipulation on the vertex variable
            //for this we'll need to loop through all gravitational bodies in the system and get their action contribution (Kinetic - Potential)
            float totalPotential = 0;
            foreach (GameObject a in celestialObjects)
            {
                //for each celestial object we'll need the gravitational potential caused at the vertex point
                float U = (G * a.GetComponent<Rigidbody>().mass/((a.transform.position - vertex).magnitude));

                totalPotential += U;
            }

            //now perform the deformation of the vertex:
            //for now we'll simply make the deformationproportional to the lagrangian at that point with some arbitrary constant:
            vertex.y = Mathf.Clamp(-totalPotential * k, -5000, 5000);
            //new deformed vertex replaces old vertex in vertex list
            vertices[i] = vertex;
        }
    }

}
