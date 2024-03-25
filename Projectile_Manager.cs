using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile_Manager : MonoBehaviour
{
    //projectile variables:
    
    public float velocity; ///escape velocity
    public float thrust;
    public Transform point_of_origin;
    public GameObject projectile;
    public float mass_of_projectile;
    public float Launch_Angle;
    public Slider slider;
    //[SerializeField] Vector2 Direction = new Vector3 (1, 1);
    // Start is called before the first frame update

    public void OnSliderValueChanged()
    {
        Launch_Angle = slider.value;
        //set the angle of the point of origin to the angle of the UI Slider (in the y-direction)
        point_of_origin.GetComponent<Transform>().Rotate(0, Launch_Angle, 0, Space.World);
    }
    
    //when fire button is pressed
    public void ButtonPressed()
    {
        
        //make it so that the point of origin will be based on user inputs, and then destroy itself.
        //if input is pressed fire a projectile from the origin point with velocity
            GameObject projectile_instance = Instantiate(projectile, point_of_origin.GetComponent<Transform>().position,
                                                         point_of_origin.GetComponent<Transform>().rotation);
            projectile_instance.tag = "celestial_body";
            projectile_instance.GetComponent<Rigidbody>().mass = mass_of_projectile;
            
            projectile_instance.GetComponent<Rigidbody>().AddForce(point_of_origin.forward * thrust, ForceMode.Impulse);
    }
}
