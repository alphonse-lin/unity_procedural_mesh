using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ConnectWithJS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [DllImport("__Internal")]
    private static extern void clickSelectFileBtn();
    /// <summary>
    /// 点击Open按钮
    /// </summary>
    public static void ClickSelectFileBtn()
    {
        clickSelectFileBtn();
    }
}
