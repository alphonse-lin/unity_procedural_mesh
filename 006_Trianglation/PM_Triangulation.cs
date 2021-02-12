using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;

namespace PM.Application
{
    public class PM_Triangulation : MonoBehaviour
    {
        void Start()
        {
            // Create Vector2 vertices
            Vector2[] vertices2D = new Vector2[] {
            new Vector2(0,0),
            new Vector2(0,45),
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

            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
            }

            // Create the mesh
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            // Set up game object with mesh;
            gameObject.AddComponent(typeof(MeshRenderer));
            MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
            filter.mesh = msh;
        }
    }
}
