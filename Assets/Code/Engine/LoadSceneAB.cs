using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneAB : MonoBehaviour {

    AssetBundle _rootBundle;
    AssetBundleManifest _manifest;
    string kABDir = "";
    List<AssetBundle> _bundleList = new List<AssetBundle>();

    void Start()
    {
        kABDir = Application.streamingAssetsPath + "/ABs/";
        string mainManifestPath = kABDir + "ABs";
        _rootBundle = AssetBundle.LoadFromFile(mainManifestPath);
        _manifest = _rootBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        GameObject.DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        _rootBundle.Unload(true);
    }

    // 直接读取streamingAssets目录
    void ChangeScene()
    {
        //LoadScene("1001_Test", "1001_Test_Scene");
        LoadScene("1001", "1001_Scene");
    }

    void LoadScene(string sceneName, string sceneBundleName)
    {
        List<AssetBundle> dependBundleList = new List<AssetBundle>();
        string[] dependPath = _manifest.GetAllDependencies(sceneBundleName);
        for (int i = 0; i < dependPath.Length; i++)
        {
            Debug.Log(dependPath[i]);
            var dependBundle = AssetBundle.LoadFromFile(kABDir + dependPath[i]);
            dependBundleList.Add(dependBundle);
        }
        

        var sceneBundle = AssetBundle.LoadFromFile(kABDir + sceneBundleName);
        if (sceneBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        _bundleList.AddRange(dependBundleList.ToArray());
        _bundleList.Add(sceneBundle);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // 场景的话，这里的bundle不能unload,unload就有问题了
        //for (int i = 0; i < dependBundleList.Count; i++)
        //{
        //    dependBundleList[i].Unload(false);
        //}
        //sceneBundle.Unload(false);
    }

    void UnLoadSceneBundle()
    {
        for (int i = 0; i < _bundleList.Count; i++)
        {
            _bundleList[0].Unload(false);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("xxxxxx " + level);
        UnLoadSceneBundle();
    }

    //void ChangeScene2()
    //{
    //    StartCoroutine(Download());
    //}


    //IEnumerator Download()
    //{
    //    string url = "file://" + Application.streamingAssetsPath + "/Scene.unity3d";

    //    WWW www = new WWW(url);
    //    yield return www;
    //    if (www.error != null)
    //    {
    //        Debug.Log("下载失败");
    //    }
    //    else
    //    {
    //        AssetBundle bundle = www.assetBundle;
    //        Application.LoadLevel("1001");
    //        print("跳转场景");
    //        // AssetBundle.Unload(false)，释放AssetBundle文件内存镜像，不销毁Load创建的Assets对象
    //        // AssetBundle.Unload(true)，释放AssetBundle文件内存镜像同时销毁所有已经Load的Assets内存镜像
    //        // 这里会报错
    //        //bundle.Unload(false);
    //    }

    //    // 中断正在加载过程中的WWW
    //    www.Dispose();
    //}

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Next"))
        {
            ChangeScene();
        }
    }
}
