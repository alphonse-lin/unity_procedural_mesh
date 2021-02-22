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
        public static Vector2[][] ReadGeoJSONGeometry(string jsonFilePath)
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

        public static Vector2[] JSONPtToVector2(Geometry GeoDic)
        {
            var resultCount = GeoDic.Coordinates.Length;
            Vector2[] vectArray = new Vector2[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                var x = float.Parse((GeoDic.Coordinates[i].X - 446144).ToString());
                var y = float.Parse((GeoDic.Coordinates[i].Y - 4412713).ToString());
                //Debug.Log($"{x},{y}");
                vectArray[i] = new Vector2(x, y);
            }
            return vectArray;
        }
        #endregion
    }
}
