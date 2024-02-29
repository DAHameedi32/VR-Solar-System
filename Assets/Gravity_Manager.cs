using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
    Gravitation manager for the VR Solar System 3rd Year Project at KCL. 
    Coding done primarily by Mr Daniyal Ali Hameedi, with some some creative input from my close friend and collaborator Kirpal Grewal at Cambridge,
    and classmate Gabriel Fallis.

*/

//Original Plane-mesh generation Code obtained from: https://gist.github.com/goldennoodles/38be017efae5459d34120550645c73f9

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
// Requires these components for the script to actually run, will throw an error otherwise.

public class Gravity_Manager : MonoBehaviour
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

    public float k = 1.0f; //Lagrangian Deformation Constant of porportionality
    GameObject[] celestialObjects; //Array containing all celestial objects in the scene

    List<int> userDeformedVertices = new List<int>();
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
        //some notes about Fixed Update:
        /*
        Fixed Update is the physics engine update and takes higher priority to the regular update function.

        What this means is that when the computer gets in a scenario where there is a lot of compute time  being assigned to a job, it will usually either 
        - suspend some work until the next frame to keep the game running smoothly
        - split the work over multiple frames
        - or just approximate things.
        For Fixed Update however all code in this block MUST be completed before the frame updates, meaning that:
        a) this is where bottlenecks can occur, and 
        b) optimisation here is important.
        */

        //start by generating the mesh:
        planeResolution = Mathf.Clamp(planeResolution, 1, 101); //clamps the mesh resolution between some arbitrary values. This helps performance for self explanatory reasons
        GeneratePlane(planeSize, planeResolution);


        Deform(); //deforms all points in the plane according to the action of Newtonian Gravitation, this includes user-made deformations (It shouldn't)

        AssignMesh(); //clears the old mesh/meshfilter and replaces with our new updated mesh so it's drawn
        
        //apply gravitational and the deformational force to all relevant bodies
        compute_Gravity(celestialObjects); //computes the gravity and deformational force and applies them.


    }

    //gravity must be computed now as the law of gravitation + the deformation force of the mesh felt at that point due to player input.
    //as such, the arguments is the celestial objects in the scene, and the vertices comprising the action plane.
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

        //second part of the code will find the gravitation due to user's deformation of the action surface.
        //in order to do this, we will first check the current Deformation at each point against the expected Deformation due to gravity.
        //to do this we calculate the expected deformation of every vertex due to gravity:
        
        /*
        As a point of order the below code is fucking garbage.
        This is a horrible way to do this, particularly as optimisation here is a requirement.
        I have no idea whether this will actually run within the alloted fixed update time.
        I will look into using the job system to do this bit in parallel to the above bit, which would honestly make a good bit more sense, but at this point i just want this to be written.
        
        ADDENDUM: I was entirely correct and this horrible implementation lags the ever loving shit out of the simulation. It is so bad that I havent bothered to collect data on how bad it is.
        I also find it hilarious that i was able to get this to work without having implemented the system for a user to interact with the surface.
        If that isn't deserving of a medal i dont know what is.

        EDIT 03/02/2024 23:18: 
        Optimisations have been made, and it now does not "lag the ever loving fuck out of the simulation". But further improvements can be made.

        EDIT 07/02/2024 13:29:
        I refuse to multithread this. My policy on the optimal number of threads: THERE CAN ONLY BE ONE!!!
        */

        //start by declaring a variable to store the most recent gravity-deformed vertices: (moving this here temporarily)
        Vector3 lastGravVert = Vector3.zero;
        for (int i = 0; i<vertices.Count; i++)
        {
            
            float totalPotential = 0;
            float totalKinetic = 0;
            foreach (GameObject a in celestialObjects)
            {
                //calculate the deformation at a point due to gravitation from each body
                float U = (G * a.GetComponent<Rigidbody>().mass/((a.transform.position - vertices[i]).magnitude));
                float T = 0;

                totalPotential += U;
                totalKinetic += T;
            }
            float Lagrangian = totalKinetic - totalPotential;
            //store in our list of expected deformations:
            float expectedDeformation = Mathf.Clamp(Lagrangian * k, -5000, 5000);
            
            if(vertices[i].y != expectedDeformation)
            {
                //we have found a user deformed vertex!
                //we should now add this to a list of user-deformed vetrices so that the actual gravitational deformation will not affect it.
                //rather than storing the vertex itself we'll just store it's indices in a list. 
                //we can now test whether this is potential or kinetic dominated:
                //if they are potential dominated (<0) we will then want them to interact with our planets
                //if they are kinetic dominated (>0) we wont want them to interact because the KE is an intrinsic local property.
                //potential dominated lagrangian:
                if(vertices[i].y < 0)
                {
                    //calculate the gravitational force felt by body all bodies a from the potential generated by the deformed point:
                    //to do this we must estimate the mass of the hypotetical body causing a deformation:
                    float r = (vertices[i] - lastGravVert).magnitude;
                    float estimated_mass = (vertices[i].y * r )/(-1 * G);
                    //we will assume that the object itself has a Unit Mass.
                    foreach (GameObject a in celestialObjects)
                    {
                        //compute the force acted on a by the potential generat3ed by the user-made deformation
                        a.GetComponent<Rigidbody>().AddForce((vertices[i] - a.transform.position).normalized * (G * (a.GetComponent<Rigidbody>().mass * estimated_mass)/(r*r)));
                    }
                    return;
                }
                //kinetic dominated lagrangian, we include 0 here as if there is 0 potential, then any interaction with the point is trivial, as Kinetic Energy is intrinsic, and local.
                else if(vertices[i].y >= 0)
                {
                    return;
                }
            }
            else //if we have not found a user deformed vertex:
            {
                //store this vertex in lastGravVert:
                lastGravVert = vertices[i];
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

        //Create triangles:
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
            float totalKinetic = 0;
            foreach (GameObject a in celestialObjects)
            {
                 //for each celestial object we'll need the gravitational potential caused at the vertex point
                float U = (G * a.GetComponent<Rigidbody>().mass/((a.transform.position - vertex).magnitude));
                float T = 0; //we assume 0 kinetic energy at the point.

                totalPotential += U;
                totalKinetic += T;
            }
            float Lagrangian = totalKinetic - totalPotential;

            //now perform the deformation of the vertex:
            //for now we'll simply make the deformation proportional to the lagrangian at that point with some arbitrary constant:
            vertex.y += Mathf.Clamp(Lagrangian * k, -5000, 5000); //clamped between these values for consistency.
            //new deformed vertex replaces old vertex in vertex list
            vertices[i] = vertex;
        }
    }
}
