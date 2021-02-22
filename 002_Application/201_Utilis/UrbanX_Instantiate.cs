using NetTopologySuite.Geometries;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UrbanX.IO.GeoJSON;

public class UrbanX_Instantiate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public Transform parnet;

    private string jsonPath = @"E:\114_temp\018_unity\unityProjects\test_001_procedural_mesh\Assets\Data\building_32650.geojson";

    void Start()
    {
        var vertices2D = ReadGeoJSON(jsonPath);
        for (int i = 0; i < vertices2D.Length; i++)
        {
            Instantiate(prefab, new Vector3(vertices2D[i][0].x,0, vertices2D[i][0].y), Quaternion.identity,parnet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            var x = float.Parse((GeoDic.Coordinates[i].X / 100).ToString());
            var y = float.Parse((GeoDic.Coordinates[i].Y / 100).ToString());
            vectArray[i] = new Vector2(x, y);
            Debug.Log(x);
        }
        return vectArray;
    }
    #endregion
}
