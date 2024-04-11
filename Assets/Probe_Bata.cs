using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Probe_Bata : MonoBehaviour
{
    private string fileName = "data.txt"; // Name of the text file to write to
    
    public float creationTime;
    public List<string> Velocity_Data = new List<string>();
    public string filePath;
    void Start()
    {
        // Record the time when the object is instantiated
        creationTime = Time.time;
        filePath = Path.Combine("C:/Users/Daniyal/AppData/LocalLow/DefaultCompany/VR Solar System", fileName);
        Debug.Log("Writing to path:" + filePath);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //collects all velocity data at a given time.
        float timeSinceCreation = Time.time - creationTime;
        (float, Vector3, Vector3) Buffer = (timeSinceCreation, this.GetComponent<Rigidbody>().velocity, this.GetComponent<Transform>().position);
        Velocity_Data.Add(Buffer.ToString());

    }
    public void Collect_Data()
    {
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            foreach(string item in Velocity_Data)
            {
                writer.WriteLine(item);
            }
        }
        Debug.Log("List contents written to file: " + filePath);
    }

}
