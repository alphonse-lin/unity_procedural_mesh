using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM.GeometryClass;
using System;
using System.IO;
using UrbanX.IO.GeoJSON;
using NetTopologySuite.Geometries;

namespace PM.Application
{
    //[ExecuteInEditMode]
    public class PM_Mesh4 : MonoBehaviour
    {
        [Header("Mesh Options")]
        [Min(0)]
        [SerializeField]
        public float extrudeDis = 10f;
        public Material meshMaterial;

        //private string jsonPath = @"E:\114_temp\018_unity\unityProjects\test_001_procedural_mesh\Assets\Data\building_4326.geojson";
        private string jsonPath = @"E:\114_temp\018_unity\unityProjects\test_001_procedural_mesh\Assets\Data\building_32650.geojson";
        private bool invertFaces = true;
        private MeshExtrusion.Edge[] precomputedEdges;

        //TODO: 改写成Method
        public void Start()
        {
            var vertices2D = ReadGeoJSON(jsonPath);
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
            extrudeMesh.Optimize();
            //ApplyMeshColor(extrudeMesh);
            filter.mesh = extrudeMesh;
        }

        //TODO: 更新Dis
        private void Update()
        {

        }

        #region 创建mesh
        private Matrix4x4[] CreateHeight(float height)
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
            Color[] colors = new Color[count];

            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
            }
            mesh.colors = colors;
        }

        private void ApplyMeshMaterial(Mesh mesh)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            //Material newMat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            Material newMat = meshMaterial;
            mr.material = newMat;
        }
        #endregion

        #region 读取geojson
        private Vector2[] ReadGeoJSON(string jsonFilePath)
        {
            StreamReader sr = File.OpenText(jsonFilePath);

            var feactureCollection = GeoJsonReader.GetFeatureCollectionFromJson(sr.ReadToEnd());

            //var AttributeDic = feactureCollection[i].Attributes["function"];
            var GeoDic = feactureCollection[0].Geometry;
            var jsonResult = JSONPtToVector2(GeoDic);
            return jsonResult;
        }

        private Vector2[] JSONPtToVector2(Geometry GeoDic)
        {
            var resultCount = GeoDic.Coordinates.Length;
            Vector2[] vectArray = new Vector2[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                var x = float.Parse((GeoDic.Coordinates[i].X/100).ToString());
                var y = float.Parse((GeoDic.Coordinates[i].Y/100).ToString());
                vectArray[i] = new Vector2(x, y);
                Debug.Log(x);
            }
            return vectArray;
        }
        #endregion
    }
}
