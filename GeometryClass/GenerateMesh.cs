using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PM.GeometryClass
{
    public class GenerateMesh
    {
        /// <summary>
        /// Generate Triangle Mesh in 0,0,0, size 1
        /// </summary>
        public static Mesh MakeSimpleTriMesh()
        {
            Mesh result = new Mesh();
            //Create an array of vertices
            result.vertices = new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) };
            //Create an array of inters
            result.triangles = new int[] { 0, 1, 2 };
            return result;
        }

        /// <summary>
        /// Generate Quad Mesh in 0,0,0, size 1
        /// </summary>
        public static Mesh MakeSimpleQuadMesh()
        {
            Mesh result = new Mesh();
            //Cstaireate an array of vertices
            var vertices = new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1) };
            //Create an array of inters
            var triangles = new int[] { 0, 1, 2, 2, 1, 3 };

            result.vertices = vertices;
            result.triangles = triangles;
            return result;
        }

        /// <summary>
        /// Generate Discrete Quad Mesh
        /// </summary>
        public static Mesh MakeDiscreteProceduralGrid(int[] gridSize, float cellSize, Vector3 gridOffset)
        {
            var result = new Mesh();
            //set array sizes
            var vertices = new Vector3[gridSize[0] * gridSize[1] * 4];
            var triangles = new int[gridSize[0] * gridSize[1] * 6];

            //set tracker integers
            int v = 0;
            int t = 0;

            //set vertext offset
            float vertexOffset = cellSize * 0.5f;

            for (int x = 0; x < gridSize[0]; x++)
            {
                for (int y = 0; y < gridSize[1]; y++)
                {
                    Vector3 cellOffset = new Vector3(x * cellSize, 0, y * cellSize);

                    //populate the vertices and triangles arrays
                    vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                    vertices[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + cellOffset + gridOffset;
                    vertices[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                    vertices[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + cellOffset + gridOffset;

                    triangles[t + 0] = v;
                    triangles[t + 1] = triangles[t + 4] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + 2;
                    triangles[t + 5] = v + 3;

                    v += 4;
                    t += 6;
                }
            }
            result.vertices = vertices;
            result.triangles = triangles;

            return result;
        }

        /// <summary>
        /// Generate Continuous Quad Mesh
        /// </summary>
        public static Mesh MakeContinuousProceduralGrid(int[] gridSize, float cellSize, Vector3 gridOffset)
        {
            var result = new Mesh();
            //set array sizes
            var vertices = new Vector3[(gridSize[0] + 1) * (gridSize[1] + 1)];
            var triangles = new int[gridSize[0] * gridSize[1] * 6];

            //set tracker integers
            int v = 0;
            int t = 0;

            //set vertext offset
            float vertexOffset = cellSize * 0.5f;

            //create vertex grid
            for (int x = 0; x <= gridSize[0]; x++)
            {
                for (int y = 0; y <= gridSize[1]; y++)
                {
                    vertices[v] = new Vector3((x * cellSize) - vertexOffset + gridOffset.x, gridOffset.z, (y * cellSize) - vertexOffset + gridOffset.y);
                    //vertices[v] = new Vector3((x * cellSize) - vertexOffset+ gridOffset.x, (x*y)*0.2f, (y * cellSize) - vertexOffset+gridOffset.y);
                    v++;
                }
            }

            //reset vertex tracker
            v = 0;

            //setting each cell's triangles
            for (int x = 0; x < gridSize[0]; x++)
            {
                for (int y = 0; y < gridSize[1]; y++)
                {
                    triangles[t] = v;
                    triangles[t + 1] = triangles[t + 4] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (gridSize[1] + 1);
                    triangles[t + 5] = v + (gridSize[1] + 1) + 1;
                    v++;
                    t += 6;
                }
                v++;
            }

            result.vertices = vertices;
            result.triangles = triangles;

            return result;
        }

        /// <summary>
        /// Generate Cube
        /// </summary>
        public static void MakeCube(out Vector3[] verArray, out int[] intArray)
        {
            var verticesList = new List<Vector3>();
            var trianglesList = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                MakeFace(i, out List<Vector3> vertices, out List<int> triangles);
                verticesList.AddRange(vertices);
                trianglesList.AddRange(triangles);
            }
            verArray = verticesList.ToArray();
            intArray = trianglesList.ToArray();

            /// <summary>
            /// Generate Face
            /// </summary>
            void MakeFace(int dir,out  List<Vector3> vertices, out List<int> triangles)
            {
                Mesh result = new Mesh();
                verticesList.AddRange(CubeMeshData.faceVertices(dir));
                int vCount = verticesList.Count;

                trianglesList.Add(vCount - 4);
                trianglesList.Add(vCount - 4 + 1);
                trianglesList.Add(vCount - 4 + 2);
                trianglesList.Add(vCount - 4);
                trianglesList.Add(vCount - 4 + 2);
                trianglesList.Add(vCount - 4 + 3);

                vertices = verticesList;
                triangles = trianglesList;
            }
        }

       
        /// <summary>
        /// Update Mesh
        /// </summary>
        public static void UpdateMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
        {
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }



    }

    public static class CubeMeshData
    {
        public static Vector3[] vertices =
        {
            new Vector3( 1, 1, 1),
            new Vector3(-1, 1, 1),
            new Vector3(-1,-1, 1),
            new Vector3( 1,-1, 1),

            new Vector3(-1, 1,-1),
            new Vector3( 1, 1,-1),
            new Vector3( 1,-1,-1),
            new Vector3(-1,-1,-1),
        };

        public static int[][] faceTriangles =
        {
            new int[]{ 0, 1, 2, 3},
            new int[]{ 5, 0, 3, 6},
            new int[]{ 4, 5, 6, 7},
            new int[]{ 1, 4, 7, 2},
            new int[]{ 5, 4, 1, 0},
            new int[]{ 3, 2, 7, 6},
        };

        public static Vector3[] faceVertices(int dir)
        {
            Vector3[] fv = new Vector3[4];
            for (int i = 0; i < fv.Length; i++)
            {
                fv[i] = vertices[faceTriangles[dir][i]];
            }
            return fv;
        }
    }
}
