using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Requires these components for the script to actually run, will throw an error otherwise.

public class Working_Gravitation_Manager : MonoBehaviour
{
    public float G = 100f; //Gravitational constant
    public float w = 1.0f; //Omega (Angular velocity)

    GameObject[] celestialObjects; //Array containing all celestial objects in the scene
    GameObject[] satellites; //Array containing all satellites in the scene
    // Start is called before the first frame update
    void Start()
    {
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        InitialVelocity();
    }

    //Update called once every frame
    void Update()
    {
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
    }

    //Fixed Update called once every 0.035 seconds
    void FixedUpdate()
    {
        //apply gravitational force to all relevant bodies
        compute_Gravity(celestialObjects);
        //make this return the gravitation at every 
        
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
                    a.GetComponent<Rigidbody>().angularVelocity += Vector3.up * w; //N.B. Arbitrary for now
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

}
