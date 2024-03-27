using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class look_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //constantly rotates itself to look in the local forward direction:
        this.GetComponent<Transform>().Rotate(this.GetComponent<Transform>().worldToLocalMatrix.MultiplyVector(Vector3.forward));
    }
}
