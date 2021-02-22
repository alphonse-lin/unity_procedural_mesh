using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
        filePath = EditorUtility.OpenFilePanel("select geojson file", "", "geojson");
    }
}
