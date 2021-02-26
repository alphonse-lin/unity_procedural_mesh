using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestObj : MonoBehaviour
{
    private byte[] readResult;
    public string filePath;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetBase64(string base64Str)
    {
        filePath = base64Str;
        Debug.Log("得到数据了");
    }
}
