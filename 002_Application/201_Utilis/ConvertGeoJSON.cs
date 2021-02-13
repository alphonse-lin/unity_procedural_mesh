using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UrbanX.IO.GeoJSON;

public class ConvertGeoJSON : MonoBehaviour
{
    public string jsonFilePath = @"E:\114_temp\008_代码集\002_extras\smallCharpTool\Application\data\geojson\building.geojson";

    void Start()
    {
        Debug.Log("Start");
        StreamReader sr = File.OpenText(jsonFilePath);

        var feactureCollection = GeoJsonReader.GetFeatureCollectionFromJson(sr.ReadToEnd());

        for (int i = 0; i < feactureCollection.Count; i++)
        {
            var jsonDic = feactureCollection[i].Attributes["function"];
            Debug.Log(jsonDic.ToString());
        }
    }
}
