using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralContinousGrid : MonoBehaviour
{
    Mesh mesh;

    //grid settings
    public float cellSize=1;
    public Vector3 gridOffset=new Vector3(0,0,0);
    public int gridSizeX=10;
    public int gridSizeY=10;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        var result=GenerateMesh.MakeDiscreteProceduralGrid(new int[] { gridSizeX, gridSizeY }, cellSize, gridOffset);
        GenerateMesh.UpdateMesh(mesh,result.vertices, result.triangles);
    }
}
