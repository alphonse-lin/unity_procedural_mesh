using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.BuildingClass;
using System;
using PM.GeometryClass;
using PM.ConvertClass;
using System.Linq;


public class BuildingEngine : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter filter;
    private string jsonPath = @"E:\114_temp\018_unity\unityProjects\test_001_procedural_mesh\Assets\Data\building_32650.geojson";
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        //// Create GameObject that will hold a Chunk
        var buildingsGameObject = new GameObject("Chunk 0,0,0");
        buildingsGameObject.transform.parent = transform.parent;

        buildingsGameObject.AddComponent<MeshFilter>();
        buildingsGameObject.AddComponent<MeshRenderer>();
        buildingsGameObject.GetComponent<MeshRenderer>().material = material;

        filter = buildingsGameObject.GetComponent<MeshFilter>();
        
        var vertices2D= ConvertClass.ReadGeoJSONGeometry(jsonPath);

        mesh = RenderToMesh(vertices2D);
        filter.mesh = mesh;

    }

    private Mesh RenderToMesh(Vector2[][] jsonResult, bool invertFace=true)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        int triCount = 0;
        for (int i = 0; i < jsonResult.Length; i++)
        {
            var singleMesh = GenerateMesh.ExtrudeMesh(jsonResult[i], 10f, invertFace);
            vertices.AddRange(singleMesh.vertices);
            normals.AddRange(singleMesh.normals);

            var temp_triCount = singleMesh.triangles.Length;
            for (int j = 0; j < temp_triCount; j++)
            {
                triangles.Add(singleMesh.triangles[j]+triCount);
            }
            triCount += singleMesh.triangles.Max()+1;
            
            Debug.Log(singleMesh.bounds.center);
        }
        var allMesh = new Mesh();
        int[] test = new int[2];
        allMesh.vertices = vertices.ToArray();
        allMesh.triangles = triangles.ToArray();
        allMesh.normals = normals.ToArray();
        return allMesh;
    }
}
