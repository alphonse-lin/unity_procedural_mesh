using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour
{
    Mesh mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        GenerateMesh.MakeCube(out Vector3[] verArray, out int[] intArray);
        GenerateMesh.UpdateMesh(mesh, verArray, intArray);
    }
}
