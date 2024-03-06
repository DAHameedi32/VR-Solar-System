using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counteract_Angular_Velocity : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation of the child relative to the parent
        initialRotation = Quaternion.Inverse(transform.parent.rotation) * transform.rotation;
    }

    void LateUpdate()
    {
        // Apply the opposite rotation to the child to counteract the parent's rotation
        transform.rotation = transform.parent.rotation * initialRotation;
    }
}
