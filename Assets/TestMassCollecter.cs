using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestMassCollecter : MonoBehaviour
{
    public GameObject TestMass;
    public bool Collect_Data = false;
    private List<Vector3> velocityData = new List<Vector3>();
    private List<Vector3> positionData = new List<Vector3>();
    public string velocityFileName = "velocity_data.txt"; // Name of the text file to write to
    public string positionFileName = "position_data.txt";
    void FixedUpdate()
    {   
        velocityData.Add(TestMass.GetComponent<Rigidbody>().velocity);
        positionData.Add(TestMass.GetComponent<Rigidbody>().position);
        if(Collect_Data == true)
        {
            Collect_Data = false;
            Dump_Data();
        }
    }
    void Dump_Data()
    {
         // Path to the file
        string vfilePath = Path.Combine(Application.persistentDataPath, velocityFileName);
        string pfilePath = Path.Combine(Application.persistentDataPath, positionFileName);
        // Open a stream writer to write to the file
        using (StreamWriter vwriter = new StreamWriter(vfilePath))
        {
            foreach (Vector3 velocity in velocityData)
            {
                // Write the velocity to it's file
                vwriter.WriteLine(velocity);
            }
  
        }
        using (StreamWriter pwriter = new StreamWriter(pfilePath))
        {
            foreach (Vector3 position in positionData)
            {
                //write position to it's file
                pwriter.WriteLine(position);
            }
        }
        Debug.Log("Data dumped to file: " + vfilePath);
        Debug.Log("Data dumped to file: " + pfilePath);
    }
}
