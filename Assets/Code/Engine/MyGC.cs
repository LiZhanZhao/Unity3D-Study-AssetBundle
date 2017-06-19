using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGC : MonoBehaviour {

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 100, 100, 100), "GC"))
        {
            DoGC();
        }
    }

    void DoGC()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        
    }
}
