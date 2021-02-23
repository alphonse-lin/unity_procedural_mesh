using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestObj : MonoBehaviour
{
    private byte[] readResult;
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
        byte[] bs = Convert.FromBase64String(base64Str);
        foreach (var item in bs)
        {
            Debug.Log(item);
        }
    }
}
