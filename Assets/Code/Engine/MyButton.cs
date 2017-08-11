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
            //InstanceGo();
            InstanceSpineGo();
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

    void InstanceSpineGo()
    {
        UnityEngine.Profiling.Profiler.BeginSample("MyPieceOfCode 111");
        Debug.Log("111111111");
        UnityEngine.Profiling.Profiler.EndSample();

        LoadSceneMgr loadSceneMgr = GameObject.FindObjectOfType<LoadSceneMgr>();

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        AssetBundle ab = null;
        float preTime = 0.0f;
        string prefabAssetPath = "";
        Object prefab = null;
        GameObject go;
        List<GameObject> goList = new List<GameObject>();

        preTime = Time.realtimeSinceStartup;
        ab = loadSceneMgr.LoadBundle("binary_SkeletonGraphic");
        prefabAssetPath = "Assets/res/Prefabs/binary_SkeletonGraphic.prefab";
        prefab = ab.LoadAsset<Object>(prefabAssetPath);
        Debug.Log(string.Format(" {0} Load Bundle Time : {1} ", "binary_SkeletonGraphic",
            Time.realtimeSinceStartup - preTime));


        preTime = Time.realtimeSinceStartup;
        go = Instantiate(prefab) as GameObject;

        Debug.Log(string.Format(" {0} Instantiate Time : {1} ", "binary_SkeletonGraphic",
            Time.realtimeSinceStartup - preTime));

        go.transform.SetParent(canvas.transform, false);


        UnityEngine.Profiling.Profiler.BeginSample("MyPieceOfCode 222");
        Debug.Log("222222");
        UnityEngine.Profiling.Profiler.EndSample();

        /*
        LoadSceneMgr loadSceneMgr = GameObject.FindObjectOfType<LoadSceneMgr>();
        
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        AssetBundle ab = null;
        float preTime = 0.0f;
        string prefabAssetPath = "";
        Object prefab = null;
        GameObject go;
        List<GameObject> goList = new List<GameObject>();


        preTime = Time.realtimeSinceStartup;
        ab = loadSceneMgr.LoadBundle("binary_SkeletonGraphic");
        prefabAssetPath = "Assets/res/Prefabs/binary_SkeletonGraphic.prefab";
        prefab = ab.LoadAsset<Object>(prefabAssetPath);
        Debug.Log(string.Format(" {0} Load Bundle Time : {1} ", "binary_SkeletonGraphic",
            Time.realtimeSinceStartup - preTime));


        preTime = Time.realtimeSinceStartup;
        go = Instantiate(prefab) as GameObject;

        //for (int i = 0; i < 100; i++)
        //{
        //    goList.Add(Instantiate(prefab) as GameObject);
        //}

        Debug.Log(string.Format(" {0} Instantiate Time : {1} ", "binary_SkeletonGraphic", 
            Time.realtimeSinceStartup - preTime));


        go.transform.SetParent(canvas.transform, false);

        //for (int i = 0; i < 100; i++)
        //{
        //    goList[i].transform.SetParent(canvas.transform, false);
        //}




        preTime = Time.realtimeSinceStartup;
        ab = loadSceneMgr.LoadBundle("json_SkeletonGraphic");
        prefabAssetPath = "Assets/res/Prefabs/json_SkeletonGraphic.prefab";
        prefab = ab.LoadAsset<Object>(prefabAssetPath);
        Debug.Log(string.Format(" {0} Load Bundle Time : {1} ", "json_SkeletonGraphic",
            Time.realtimeSinceStartup - preTime));


        preTime = Time.realtimeSinceStartup;
        go = Instantiate(prefab) as GameObject;
        //goList = new List<GameObject>();
        //for (int i = 0; i < 100; i++)
        //{
        //    goList.Add(Instantiate(prefab) as GameObject);
        //}
        Debug.Log(string.Format(" {0} Instantiate Time : {1} ", "json_SkeletonGraphic",
            Time.realtimeSinceStartup - preTime));

        go.transform.SetParent(canvas.transform, false);
        //for (int i = 0; i < 100; i++)
        //{
        //    goList[i].transform.SetParent(canvas.transform, false);
        //}

        */
    }
}
