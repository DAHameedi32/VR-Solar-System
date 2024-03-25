using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counteract_Angular_Velocity : MonoBehaviour
{
    public Transform objectToFollow;
 
    void Update () 
    {
        transform.position = objectToFollow.position;
    }
}
