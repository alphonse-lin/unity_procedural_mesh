using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;
using System;

namespace PM.Application
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PM_Mesh2 : MonoBehaviour
    {
        private bool invertFaces = true;
        private MeshExtrusion.Edge[] precomputedEdges;

        private Matrix4x4[] matrix = {
            new Matrix4x4(
            new Vector4(1,0,0,0),
            new Vector4(0,1,0,0),
            new Vector4(0,0,1,0),
            new Vector4(0,0,0,1)),

            new Matrix4x4(
            new Vector4(1,0,0,0),
            new Vector4(0,1,0,0),
            new Vector4(0,0,1,0),
            new Vector4(0,10,0,1))
            };

        // Create Vector2 vertices
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

            //new Vector2(0,0),
            //new Vector2(0,50),
            //new Vector2(50,50),
            //new Vector2(50,0)
        };

        // Start is called before the first frame update
        void Start()
        {
            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

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
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.uv = uvs;

            msh.RecalculateNormals();
            msh.RecalculateBounds();

            var srcMesh = msh;
            precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
            MeshExtrusion.ExtrudeMesh(srcMesh, GetComponent<MeshFilter>().mesh, matrix, precomputedEdges, invertFaces);

            ////Set up game object with mesh;
            //gameObject.AddComponent(typeof(MeshRenderer));
            //MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
            //filter.mesh = msh;
        }

    }
}
