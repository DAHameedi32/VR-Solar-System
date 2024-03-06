using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Manager : MonoBehaviour
{
    //projectile variables:
    
    public float velocity; ///escape velocity
    public float thrust;
    public Transform point_of_origin;
    public GameObject projectile;
    public float mass_of_projectile;
    public bool isFired = false;
    //[SerializeField] Vector2 Direction = new Vector3 (1, 1);
    // Start is called before the first frame update
    void Start()
    {
        isFired = false;
    }

    // Update is called once per frame
    public void ButtonPressed()
    {
        
        //make it so that the point of origin will be based on user inputs, and then destroy itself.
        //if input is pressed fire a projectile from the origin point with velocity
       
            GameObject projectile_instance = Instantiate(projectile, point_of_origin.GetComponent<Transform>().position,
                                                         point_of_origin.GetComponent<Transform>().rotation);
            projectile_instance.tag = "celestial_body";
            projectile_instance.GetComponent<Rigidbody>().mass = mass_of_projectile;

            projectile_instance.GetComponent<Rigidbody>().AddForce(Vector3.forward * thrust, ForceMode.Impulse);
            isFired = false;
    }
}
