﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;
using System;

namespace PM.Application
{
    public class PM_Mesh3 : MonoBehaviour
    {
        public float extrudeDis = 10f;

        private bool invertFaces = true;
        private MeshExtrusion.Edge[] precomputedEdges;
        private Vector2[] vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0,50),
            new Vector2(50,50),
            new Vector2(50,100),
            new Vector2(0,100),
            new Vector2(0,150),
            new Vector2(150,150),
            new Vector2(150,100),
            new Vector2(100,100),
            new Vector2(100,50),
            new Vector2(150,50),
            new Vector2(150,0),
        };

        //TODO: 改写成Method
        public void Start()
        {
            // Use the triangulator to get indices for creating triangles
            Triangulator tri = new Triangulator(vertices2D);
            int[] indices = tri.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
                uvs[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
            }

            // Create the mesh
            Mesh msh = new Mesh();
            Mesh extrudeMesh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.uv = uvs;

            msh.RecalculateNormals();
            msh.RecalculateBounds();

            var srcMesh = msh;
            precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
            var heightMatrix = CreateHeight(extrudeDis);
            MeshExtrusion.ExtrudeMesh(srcMesh, extrudeMesh, heightMatrix, precomputedEdges, invertFaces);

            //Set up game object with mesh;
            gameObject.AddComponent(typeof(MeshRenderer));
            MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;

            ApplyMeshMaterial(extrudeMesh);
            ApplyMeshColor(extrudeMesh);
            filter.mesh = extrudeMesh;
        }

        //TODO: 更新Dis
        private void Update()
        {
            
        }

        private Matrix4x4[] CreateHeight(float height)
        {
            Vector4 disVect4 = new Vector4(0, height, 0,1);
            Matrix4x4[] matrix = {
            new Matrix4x4(
            new Vector4(1,0,0,0),
            new Vector4(0,1,0,0),
            new Vector4(0,0,1,0),
            new Vector4(0,0,0,1)),

            new Matrix4x4(
            new Vector4(1,0,0,0),
            new Vector4(0,1,0,0),
            new Vector4(0,0,1,0),
            disVect4)
            };

            return matrix;
    }

        //TODO: 读取高度，创建mesh extrusion
        private void CreateMesh()
        {

        }

        //TODO:读取点，并创建Mesh Plane
        private void MeshPt(Vector2[] verticesArray)
        {

        }

        private void ApplyMeshColor(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            var count = vertices.Length;
            Color[] colors= new Color[count];

            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
            }
            mesh.colors = colors;
        }

        private void ApplyMeshMaterial(Mesh mesh)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            Material newMat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            mr.material = newMat;
        }
    }
}
