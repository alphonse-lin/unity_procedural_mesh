using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.IO;

public class ReadWeb : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadResourceCorotine());
    }

    IEnumerator LoadResourceCorotine()
    {
        UnityWebRequest request = UnityWebRequest.Get(@"file://E://building_32650.geojson");
        yield return request.SendWebRequest();
        string str = request.downloadHandler.text;
        Debug.Log(str);
        //File.WriteAllText(@"D:\PlayerGamePackage\fish.lua.txt", str);
    }
    public static string Convert_UlongToString(ulong input)
    {
        string output;

        byte[] byValue = BitConverter.GetBytes(input);
        Array.Reverse(byValue);
        output = BitConverter.ToString(byValue);
        return output;
    }
}