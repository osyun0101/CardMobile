using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    void Start()
    {
        var mesh = new Mesh();

        var vertices = new List<Vector3> {
            new Vector3 (-1, 1, 0),
              new Vector3 (0, 0, 0),
              new Vector3 (1, 1, 0),
        };
        mesh.SetVertices(vertices);

        var triangles = new List<int> { 0, 2, 1 };
        mesh.SetTriangles(triangles, 0);
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
}
