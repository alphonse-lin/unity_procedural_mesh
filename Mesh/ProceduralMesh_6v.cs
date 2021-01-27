using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh_6v : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Update()
    {
        MakeMeshData();
        CreateMesh();
    }

    private void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void MakeMeshData()
    {
        //Create an array of vertices
        vertices = new Vector3[] { new Vector3(0, YValue.ins.yValue, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
            new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1) 
        };
        //Create an array of inters
        triangles = new int[] { 0, 1, 2, 3, 4, 5 };
    }


}
