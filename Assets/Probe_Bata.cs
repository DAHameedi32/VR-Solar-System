using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Probe_Bata : MonoBehaviour
{
    public string fileName = "data.txt"; // Name of the text file to write to
    
    public float creationTime;
    public List<(float, Vector3)> Velocity_Data = new List<(float, Vector3)>();

    void Start()
    {
        // Record the time when the object is instantiated
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //collects all velocity data at a given time.
        float timeSinceCreation = Time.time - creationTime;
        Velocity_Data.Add((timeSinceCreation, this.GetComponent<Rigidbody>().velocity));
    }
    public void Collect_Data()
    {
         string filePath = Path.Combine(Application.persistentDataPath, fileName);

             // Open a stream writer to write to the file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for(int i = 0; i<Velocity_Data.Count; i++)
                {
                    writer.WriteLine(Velocity_Data[i]);
                }
            }
        Debug.Log("Data dumped to file: " + filePath);
    }
}
