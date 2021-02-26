using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.BuildingClass;
using System;
using PM.GeometryClass;
using PM.ConvertClass;
using System.Linq;
using UnityEngine.Networking;
using System.IO;

public class BuildingEngine_2 : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter filter;
    private string json;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadResourceCorotine());
        var jsonPath = @"E:\114_temp\018_unity\build_unityProjects\building_32650.geojson";
        //var jsonPath = @"E:\test.txt";
        StreamReader sr = File.OpenText(jsonPath);
        var filePath = sr.ReadToEnd();

        var buildingsGameObject = new GameObject("Chunk 0,0,0");
        buildingsGameObject.transform.parent = transform.parent;

        buildingsGameObject.AddComponent<MeshFilter>();
        buildingsGameObject.AddComponent<MeshRenderer>();
        buildingsGameObject.GetComponent<MeshRenderer>().material = material;

        filter = buildingsGameObject.GetComponent<MeshFilter>();

        var vertices2D = ConvertClass.ReadGeoJSONGeometry_Unity_Fromjson(filePath, "brepHeight",out string[] jsonHeight);

        mesh = GenerateMesh.RenderToMesh(vertices2D, jsonHeight);
        filter.mesh = mesh;
    }

    IEnumerator LoadResourceCorotine()
    {
        UnityWebRequest request = UnityWebRequest.Get("file://building_32650.geojson");
        yield return request.SendWebRequest();
        string str = request.downloadHandler.text;
        json=str;
        Debug.Log(json);

        var buildingsGameObject = new GameObject("Chunk 0,0,0");
        buildingsGameObject.transform.parent = transform.parent;

        buildingsGameObject.AddComponent<MeshFilter>();
        buildingsGameObject.AddComponent<MeshRenderer>();
        buildingsGameObject.GetComponent<MeshRenderer>().material = material;

        filter = buildingsGameObject.GetComponent<MeshFilter>();

        var vertices2D = ConvertClass.ReadGeoJSONGeometry_Fromjson(json, "brepHeight", out string[] jsonHeight);

        mesh = GenerateMesh.RenderToMesh(vertices2D, jsonHeight);
        filter.mesh = mesh;
        //File.WriteAllText(@"D:\PlayerGamePackage\fish.lua.txt", str);
    }
}
