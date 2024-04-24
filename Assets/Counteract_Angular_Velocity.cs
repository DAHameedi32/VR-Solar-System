using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counteract_Angular_Velocity : MonoBehaviour
{
    public Slider slider;
    public Toggle toggle;
    public GameObject objectToFollow;
    
    void Start()
    {
        Vector3 vVector = objectToFollow.GetComponent<Rigidbody>().velocity;
        transform.LookAt(vVector); 
        //make the object face the velocity direction by default
        transform.position = objectToFollow.GetComponent<Transform>().position + (vVector * 0.25f);
        transform.LookAt(vVector);
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        Vector3 vVector = objectToFollow.GetComponent<Rigidbody>().velocity;
        //make the object face the velocity direction by default
        transform.position = objectToFollow.GetComponent<Transform>().position + (vVector * 0.25f);
        if(toggle.isOn)
        {
            transform.LookAt(vVector);
        }
    }
    void OnSliderValueChanged(float angle)
    {
        if(!toggle.isOn)
        {   
            // Convert slider value to radians
            float rotationAngleRadians = angle;

            // Create a rotation quaternion using Euler angles
            Quaternion rotation = Quaternion.Euler(Vector3.up * rotationAngleRadians);

            // Apply the rotation to the target object
            transform.rotation = rotation;
        }
    }

}
