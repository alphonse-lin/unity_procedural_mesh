﻿using System.Collections;
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
    private string filePath = "";
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pause());
    }

    private void Update()
    {
        
        ////// Create GameObject that will hold a Chunk
        //Debug.Log(filePath);
        //StartCoroutine(ExtrudeMesh(filePath));
    }

    IEnumerator pause()
    {
        
        while (filePath.Length<10)
        {
            Debug.Log("wait");
            filePath = GameObject.Find("ButtonTest").GetComponent<ButtonClick>().filePath;
            yield return null;
        }
        Debug.Log("now");
        yield return ExtrudeMesh(filePath);
    }
    IEnumerator ExtrudeMesh(string filePath)
    {

        if (filePath.Length >= 2)
        {
            var buildingsGameObject = new GameObject("Chunk 0,0,0");
            buildingsGameObject.transform.parent = transform.parent;

            buildingsGameObject.AddComponent<MeshFilter>();
            buildingsGameObject.AddComponent<MeshRenderer>();
            buildingsGameObject.GetComponent<MeshRenderer>().material = material;

            filter = buildingsGameObject.GetComponent<MeshFilter>();

            Debug.Log(filePath);
            var vertices2D = ConvertClass.ReadGeoJSONGeometry(filePath, "brepHeight", out string[] jsonHeight);

            mesh = GenerateMesh.RenderToMesh(vertices2D, jsonHeight);
            filter.mesh = mesh;
            yield return null;
        }
        
    }
}
