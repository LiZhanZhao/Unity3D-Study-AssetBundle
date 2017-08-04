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

        if (GUI.Button(new Rect(200, 0, 100, 100), "Empty"))
        {
            ToEmptyScene();
        }

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

    void ToEmptyScene()
    {
        // 清理MyObj.saveAsset
        MyObj myObj = GameObject.FindObjectOfType<MyObj>();
        GameObject.DestroyImmediate(myObj.saveAsset, true);

        UnityEngine.SceneManagement.SceneManager.LoadScene("empty");
    }

    void InstanceGo()
    {
        LoadSceneMgr loadSceneMgr = GameObject.FindObjectOfType<LoadSceneMgr>();
        AssetBundle ab = loadSceneMgr.LoadBundle("306025");
        string prefabAssetPath = "Assets/res/Prefabs/char/306025/306025.prefab";
        var prefab = ab.LoadAsset<Object>(prefabAssetPath);
        GameObject go = Instantiate(prefab) as GameObject;

        //MyObj myObj = GameObject.FindObjectOfType<MyObj>();
        //myObj.saveGo = go;
        //myObj.saveAsset = prefab;
    }
}
