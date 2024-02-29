using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Original Code provided by: https://gist.github.com/goldennoodles/38be017efae5459d34120550645c73f9

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Mesh_Manager : MonoBehaviour
{
    //References
    Mesh myMesh;
    MeshFilter meshFilter;

    //Plane Settings
    [SerializeField] Vector2 planeSize = new Vector2(1, 1);
    [SerializeField] int planeResolution = 1;

    //Mesh values
    List<Vector3> vertices;
    List<int> triangles;


    // Start is called before the first frame update
    void Start()
    {
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;
    }

    // Update is called once per frame
    void Update()
    {
        //clamps plane resolution between 1 and 50.
        // resolution >1 as a 0 or negative resolution would not make sense
        // resolution <50 for arbitrary performance reasons.
        planeResolution = Mathf.Clamp(planeResolution, 1, 50);

        GeneratePlane(planeSize, planeResolution);
        Deform(Time.timeSinceLevelLoad);
        AssignMesh();
    }

    void GeneratePlane(Vector2 size, int resolution)
    {
        //Create vertices:
        vertices = new List<Vector3>();
        float xPerStep = size.x/resolution;
        float yPerStep = size.y/resolution;

        for (int y = 0; y<resolution+1; y++)
        {
            for (int x = 0; x<resolution+1; x++)
            {
                vertices.Add(new Vector3(x*xPerStep, 0, y*yPerStep));
            }
        }

        triangles = new List<int>();
        for (int row = 0; row<resolution; row++)
        {
            for (int column = 0; column <resolution; column++)
            {
                int i = (row*resolution) + row + column;
                
                //first triangle
                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);

                //second triangle
                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i+1);
            }
        }
    }
    void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
    }

    void Deform(float t)
    {
        for (int i = 0; i<vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.Sin(t + vertex.x);
            vertices[i] = vertex;
        }
    }
}
