using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UrbanX.IO.GeoJSON;

namespace PM.ConvertClass
{
    public static class ConvertClass
    {
        #region 读取geojson
        public static Vector2[][] ReadGeoJSONGeometry(string jsonFilePath, string attributeName, out string[] jsonAttribute)
        {
            StreamReader sr = File.OpenText(jsonFilePath);

            var feactureCollection = GeoJsonReader.GetFeatureCollectionFromJson(sr.ReadToEnd());
            var geoCount = feactureCollection.Count;

            
            Vector2[][] jsonResultGeo = new Vector2[geoCount][];
            string[] jsonResultAttr = new string[geoCount];
            for (int i = 0; i < feactureCollection.Count; i++)
            {
                var GeoDic = feactureCollection[i].Geometry;
                var AttributeDic = feactureCollection[i].Attributes[attributeName];
                Vector2[] singleJsonResult = JSONPtToVector2(GeoDic);
                
                jsonResultGeo[i] = singleJsonResult;
                jsonResultAttr[i] = AttributeDic.ToString();
            }

            jsonAttribute = jsonResultAttr;
            return jsonResultGeo;
        }

        public static String[] ReadGeoJSONAttribute(string jsonFilePath, string attributeName)
        {
            StreamReader sr = File.OpenText(jsonFilePath);

            var feactureCollection = GeoJsonReader.GetFeatureCollectionFromJson(sr.ReadToEnd());
            var geoCount = feactureCollection.Count;
            String[] jsonResult = new String[geoCount];
            for (int i = 0; i < feactureCollection.Count; i++)
            {
                var AttributeDic = feactureCollection[i].Attributes[attributeName];
                string singleJsonResult = AttributeDic.ToString();
                jsonResult[i] = singleJsonResult;
            }
            return jsonResult;
        }


        public static Vector2[] JSONPtToVector2(Geometry GeoDic)
        {
            var resultCount = GeoDic.Coordinates.Length;
            Vector2[] vectArray = new Vector2[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                var x = float.Parse((GeoDic.Coordinates[i].X - 446144).ToString())/100;
                var y = float.Parse((GeoDic.Coordinates[i].Y - 4412713).ToString())/100;
                //Debug.Log($"{x},{y}");
                vectArray[i] = new Vector2(x, y);
            }
            return vectArray;
        }
        #endregion
    }
}
