using System;
using System.Collections.Generic;
using System.IO;
using NetTopologySuite.Geometries;
using PM.GeometryClass;
using UnityEngine;
using UrbanX.IO.GeoJSON;

namespace PM.BuildingClass
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class BuildingChunk : MonoBehaviour
    {
        private int[] _voxels = new int[700];
        private MeshFilter _meshFilter;
        private string jsonPath = @"E:\114_temp\018_unity\unityProjects\test_001_procedural_mesh\Assets\Data\building_32650.geojson";
        private int jsonCount;
        private Vector2[][] jsonResult;
        private bool invertFaces = false;

        public BuildingChunk()
        {
            jsonResult = ReadGeoJSON(jsonPath);
            jsonCount = jsonResult.Length;
        }

        public int this[int count]
        {
            get { return _voxels[count]; }
            set { _voxels[count] = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        void Update()
        {
            RenderToMesh();
        }

        private void RenderToMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            for (int i = 0; i < jsonCount; i++)
            {
                var count = this[i];
                if (count==0)
                    continue;
                var singleMesh = GenerateMesh.ExtrudeMesh(jsonResult[i], 10f, invertFaces);
                vertices.AddRange(singleMesh.vertices);
                triangles.AddRange(singleMesh.triangles);
                normals.AddRange(singleMesh.normals);
                Debug.Log(singleMesh.bounds.center);
            }
            var allMesh = new Mesh();
            allMesh.SetVertices(vertices);
            allMesh.SetTriangles(triangles.ToArray(), 0);
            allMesh.SetNormals(normals);
            _meshFilter.mesh = allMesh;

        }

        #region 读取geojson
        private Vector2[][] ReadGeoJSON(string jsonFilePath)
        {
            StreamReader sr = File.OpenText(jsonFilePath);

            var feactureCollection = GeoJsonReader.GetFeatureCollectionFromJson(sr.ReadToEnd());
            var geoCount = feactureCollection.Count;
            //var AttributeDic = feactureCollection[i].Attributes["function"];
            Vector2[][] jsonResult = new Vector2[geoCount][];
            for (int i = 0; i < feactureCollection.Count; i++)
            {
                var GeoDic = feactureCollection[i].Geometry;
                Vector2[] singleJsonResult = JSONPtToVector2(GeoDic);
                jsonResult[i] = singleJsonResult;
            }

            return jsonResult;
        }

        private Vector2[] JSONPtToVector2(Geometry GeoDic)
        {
            var resultCount = GeoDic.Coordinates.Length;
            Vector2[] vectArray = new Vector2[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                var x = float.Parse((GeoDic.Coordinates[i].X-446144).ToString());
                var y = float.Parse((GeoDic.Coordinates[i].Y-4412713).ToString());
                //Debug.Log($"{x},{y}");
                vectArray[i] = new Vector2(x, y);
            }
            return vectArray;
        }
        #endregion
    }
}
