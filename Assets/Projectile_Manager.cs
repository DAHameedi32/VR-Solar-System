using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Manager : MonoBehaviour
{
    //projectile variables:
    public float mass;
    public float velocity; //escape velocity
    public float thrust;
    public GameObject point_of_origin;
    public GameObject projectile;
    public bool isFired = false;
    // Start is called before the first frame update
    void Start()
    {
        isFired = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if input is pressed fire a projectile from the origin point with velocity
        if(isFired == true)
        {
            GameObject projectile_instance = Instantiate(projectile, point_of_origin.GetComponent<Transform>().position,
                                                         point_of_origin.GetComponent<Transform>().rotation);
            projectile_instance.tag = "celestial_body";
            projectile_instance.GetComponent<Rigidbody>().mass = mass;
            //need to get the force from escape velocity. hmmmm.

            projectile_instance.GetComponent<Rigidbody>().AddForce(Vector3.forward * thrust);
            isFired = false;
        }
    }
}
