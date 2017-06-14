using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAB : MonoBehaviour {

    // Use this for initialization  
    void Start()
    {

        string ABDir = Application.streamingAssetsPath + "/ABs/";

        string mainManifestPath = ABDir + "ABs";
        var rootBundle = AssetBundle.LoadFromFile(mainManifestPath);
        AssetBundleManifest manifest = rootBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string prefabBundleName = "111";
        //dependBundle  
        List<AssetBundle> dependBundleList = new List<AssetBundle>();
        string[] dependPath = manifest.GetAllDependencies(prefabBundleName);
        for (int i = 0; i < dependPath.Length; i++)
        {
            Debug.Log(dependPath[i]);
            var dependBundle = AssetBundle.LoadFromFile(ABDir + dependPath[i]);
            dependBundleList.Add(dependBundle);
        }

        var myLoadedAssetBundle = AssetBundle.LoadFromFile(ABDir + prefabBundleName);
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        string prefabAssetPath = "Assets/res/Prefabs/char/311009/311009.prefab";
        var prefab = myLoadedAssetBundle.LoadAsset<Object>(prefabAssetPath);
        Instantiate(prefab);

        // unload  
        for (int i = 0; i < dependBundleList.Count; i++)
        {
            dependBundleList[i].Unload(false);
            //dependBundleList[i].Unload(true);
        }
        myLoadedAssetBundle.Unload(false);
        //myLoadedAssetBundle.Unload(true);
    }
}
