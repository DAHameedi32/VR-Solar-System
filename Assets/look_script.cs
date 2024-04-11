using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look_script : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

         // Calculate the rotation to align with the velocity vector
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity.normalized);

        // Apply the rotation to align the object with the velocity vector
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
