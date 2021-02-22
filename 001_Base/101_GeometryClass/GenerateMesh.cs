using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PM.GeometryClass
{
    public class GenerateMesh : MonoBehaviour
    {
        #region simple mesh
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

        #endregion

        public static Mesh RenderToMesh(Vector2[][] jsonResult, string[] jsonHeight, bool invertFace = true)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            int triCount = 0;
            for (int i = 0; i < jsonResult.Length; i++)
            {
                var singleMesh = GenerateMesh.ExtrudeMesh(jsonResult[i], float.Parse(jsonHeight[i])/100, invertFace);
                vertices.AddRange(singleMesh.vertices);
                normals.AddRange(singleMesh.normals);

                var temp_triCount = singleMesh.triangles.Length;
                for (int j = 0; j < temp_triCount; j++)
                {
                    triangles.Add(singleMesh.triangles[j] + triCount);
                }
                triCount += singleMesh.triangles.Max() + 1;

                Debug.Log(singleMesh.bounds.center);
            }
            var allMesh = new Mesh();
            int[] test = new int[2];
            allMesh.vertices = vertices.ToArray();
            allMesh.triangles = triangles.ToArray();
            allMesh.normals = normals.ToArray();
            return allMesh;
        }

        /// <summary>
        /// 读取高度，创建mesh extrusion
        /// </summary>
        /// <returns>Mesh</returns>
        public static Mesh ExtrudeMesh(Vector2[] vertices2D, float extrudeDis, bool invertFaces= true)
        {
            Mesh extrudeMesh = new Mesh();

            var srcMesh = BoundaryPlaneFromPt(vertices2D);

            var precomputedEdges = MeshExtrusion.BuildManifoldEdges(srcMesh);
            var heightMatrix = CreateHeight(extrudeDis);
            MeshExtrusion.ExtrudeMesh(srcMesh, extrudeMesh, heightMatrix, precomputedEdges, invertFaces);
            return extrudeMesh;
        }

        private static Matrix4x4[] CreateHeight(float height)
        {
            Vector4 disVect4 = new Vector4(0, height, 0, 1);
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

        /// <summary>
        /// 读取点，并创建Mesh Plane
        /// </summary>
        /// <returns>Mesh</returns>
        public static Mesh BoundaryPlaneFromPt(Vector2[] temp_vertices2D)
        {
            // Use the triangulator to get indices for creating triangles
            var vertices2D = new Vector2[temp_vertices2D.Length-1];
            for (int i = 0; i < temp_vertices2D.Length-1; i++)
            {
                vertices2D[i] = temp_vertices2D[i];
            }
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
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.uv = uvs;

            msh.RecalculateNormals();
            msh.RecalculateBounds();
            msh.Optimize();

            return msh;
        }

        #region 通用模块
        public static Color[] GenerateDefaultMeshColor(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            var count = vertices.Length;
            Color[] colors = new Color[count];

            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
            }
            return colors;
        }

        public static Material GenerateDefaultMeshMaterial()
        {
            return  new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
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
        #endregion
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
