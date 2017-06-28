using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour {

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnGUI()
    {
        //if (GUI.Button(new Rect(0, 100, 100, 100), "GC"))
        //{
        //    DoGC();
        //}

        if (GUI.Button(new Rect(0, 100, 100, 100), "Clear Bundle Cache "))
        {
            LoadSceneMgr loadSceneMgr = GameObject.FindObjectOfType<LoadSceneMgr>();
            loadSceneMgr.UnLoadBundleCache();
        }

        if (GUI.Button(new Rect(0, 200, 100, 100), "Instance Go"))
        {
            InstanceGo();
        }
    }

    void DoGC()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        
    }

    void InstanceGo()
    {
        float preTime = Time.realtimeSinceStartup;

        LoadSceneMgr loadSceneMgr = GameObject.FindObjectOfType<LoadSceneMgr>();
        AssetBundle ab = loadSceneMgr.LoadBundle("306025");
        string prefabAssetPath = "Assets/res/Prefabs/char/306025/306025.prefab";
        var prefab = ab.LoadAsset<Object>(prefabAssetPath);
        Instantiate(prefab);

        float dt = Time.realtimeSinceStartup - preTime;
        Debug.Log(string.Format("<color=red> 加载时间为 {0} </color>", dt));

    }
}
