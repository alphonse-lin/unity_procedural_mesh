using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SFB;
using System.Runtime.InteropServices;
using PM.IO;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private Button btnSelecDir;

    public string filePath = "";
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log(filePath);
    }

    private void Update()
    {
        
    }

    public void OnBtnSelectDirClick()
    {
        //#if UNITY_EDITOR
        //        filePath = EditorUtility.OpenFilePanel("select geojson file", "", "geojson");
        //#endif
        //filePath = StandaloneFileBrowser.OpenFilePanel("Open File", "", ".geojson", true)[0];
        
    }

    public void OnBtnSelectDirClick_BackUp()
    {
        //#if UNITY_EDITOR
        //        filePath = EditorUtility.OpenFilePanel("select geojson file", "", "geojson");
        //#endif
        filePath = @"E:\114_temp\018_unity\unityProjects\test_001_procedural_mesh\Assets\Data\building_32650.geojson";

    }

    [DllImport("__Internal")]
    private static extern void clickSelectFileBtn();

    /// <summary>
    /// 点击Open按钮
    /// </summary>
    public void ClickSelectFileBtn()
    {
        clickSelectFileBtn();
    }
}
