using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Requires these components for the script to actually run, will throw an error otherwise.

public class Working_Gravitation_Manager : MonoBehaviour
{
    public float G = 1.49677869484819e-05f; //Gravitational constant
    public float w = 1.0f; //Omega (Angular velocity)

    GameObject[] celestialObjects; //Array containing all celestial objects in the scene
    // Start is called before the first frame update
    void Start()
    {
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        InitialVelocity();
    }

    //Fixed Update called once every 0.035 seconds
    void FixedUpdate()
    {
        //constantly looks for game objects with the tag
        celestialObjects = GameObject.FindGameObjectsWithTag("celestial_body");
        //apply gravitational force to all relevant bodies
        compute_Gravity(celestialObjects);
        //make this return the gravitation at every 
        
    }

    //gravity must be computed now as the law of gravitation + the deformation of the mesh at that point due to player input.
    //as such, the arguments is the celestial objects in the scene, and the 
    void compute_Gravity(GameObject[] celestialObjects)
    {
        //first part of the code computes the gravitational interactions.
        foreach (GameObject a in celestialObjects) //interactee
        {
            foreach (GameObject b in celestialObjects) //interactee
            {
                if(!a.Equals(b)) // handles the interaction of the body a with any other bodies b in the scene
                {
                    //calculate force due to gravity from Newton's law of gravitation:
                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    //compute Force:
                    Vector3 Force = (b.transform.position - a.transform.position).normalized * (G * (m1 * m2)/(r*r));

                    //if the force calculated is not non-physical then apply it:
                    
                    //checks to see if the interactee is a projectile
                    if(a.name == "Projectile 1(Clone)")
                    {
                        //if the force is >5e-01 (non-physical) then destroy it.
                        if(Force.magnitude > 5e-01)
                        {
                            if(b.name != "Earth")
                            {
                                Destroy(a);
                            }
                        }
                        else
                        {
                            a.GetComponent<Rigidbody>().AddForce(Force);
                        }
                    }  
                    else
                    {
                        //applies the calculated linear force and angular velocity to the necessary rigidbody, as well
                        a.GetComponent<Rigidbody>().AddForce(Force);
                        //applies the angular velocity
                        a.GetComponent<Rigidbody>().angularVelocity += Vector3.up * w; //N.B. Arbitrary for now
                    }
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
