using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAB : MonoBehaviour {

    // Use this for initialization  
    //void Start()
    //{

    //    string ABDir = Application.streamingAssetsPath + "/ABs/";

    //    string mainManifestPath = ABDir + "ABs";
    //    var rootBundle = AssetBundle.LoadFromFile(mainManifestPath);
    //    AssetBundleManifest manifest = rootBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

    //    string prefabBundleName = "111";
    //    //dependBundle  
    //    List<AssetBundle> dependBundleList = new List<AssetBundle>();
    //    string[] dependPath = manifest.GetAllDependencies(prefabBundleName);
    //    for (int i = 0; i < dependPath.Length; i++)
    //    {
    //        Debug.Log(dependPath[i]);
    //        var dependBundle = AssetBundle.LoadFromFile(ABDir + dependPath[i]);
    //        dependBundleList.Add(dependBundle);
    //    }

    //    var myLoadedAssetBundle = AssetBundle.LoadFromFile(ABDir + prefabBundleName);
    //    if (myLoadedAssetBundle == null)
    //    {
    //        Debug.Log("Failed to load AssetBundle!");
    //        return;
    //    }

    //    string prefabAssetPath = "Assets/res/Prefabs/char/311009/311009.prefab";
    //    var prefab = myLoadedAssetBundle.LoadAsset<Object>(prefabAssetPath);
    //    Instantiate(prefab);

    //    // 如果不调用unLoad的话，内存的Other/SerializeFile 会增加
    //    for (int i = 0; i < dependBundleList.Count; i++)
    //    {
    //        dependBundleList[i].Unload(false);
    //    }
    //    myLoadedAssetBundle.Unload(false);
    //}


    AssetBundleManifest _manifest;
    string kABDir = "";

    void Start()
    {
        kABDir = Application.streamingAssetsPath + "/ABs/";
        string mainManifestPath = kABDir + "ABs";
        var rootBundle = AssetBundle.LoadFromFile(mainManifestPath);
        _manifest = rootBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        LoadAsset("SceneRoot", "SceneRoot");
    }

    void LoadAsset(string assetPath, string bundleName)
    {
        //dependBundle  
        List<AssetBundle> dependBundleList = new List<AssetBundle>();
        string[] dependPath = _manifest.GetAllDependencies(bundleName);
        for (int i = 0; i < dependPath.Length; i++)
        {
            Debug.Log(dependPath[i]);
            var dependBundle = AssetBundle.LoadFromFile(kABDir + dependPath[i]);
            dependBundleList.Add(dependBundle);
        }

        var bundle = AssetBundle.LoadFromFile(kABDir + bundleName);
        if (bundle == null)
        {
            Debug.Log(string.Format("Failed to load {0} bundle!", bundleName));
            return;
        }

        var prefab = bundle.LoadAsset<Object>(assetPath);
        Debug.Assert(prefab != null, string.Format("{0} assetPath is null", assetPath));
        Instantiate(prefab);

        // 如果不调用unLoad的话，内存的Other/SerializeFile 会增加
        for (int i = 0; i < dependBundleList.Count; i++)
        {
            dependBundleList[i].Unload(false);
        }
        bundle.Unload(false);
    }

}
