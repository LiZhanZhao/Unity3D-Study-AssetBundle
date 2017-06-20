using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneMgr : MonoBehaviour {

    AssetBundle _rootBundle;
    AssetBundleManifest _manifest;
    List<AssetBundle> _sceneBundleList = new List<AssetBundle>();

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        InitManifest();

    }

    void InitManifest()
    {
        string mainManifestPath = Const.kPersistentABDir + "ABs";
        _rootBundle = AssetBundle.LoadFromFile(mainManifestPath);
        _manifest = _rootBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

    }

    public void LoadScene(string sceneName, string sceneBundleName)
    {
        List<AssetBundle> dependBundleList = new List<AssetBundle>();
        string[] dependPath = _manifest.GetAllDependencies(sceneBundleName);
        for (int i = 0; i < dependPath.Length; i++)
        {
            Debug.Log(dependPath[i]);
            var dependBundle = AssetBundle.LoadFromFile(Const.kPersistentABDir + dependPath[i]);
            dependBundleList.Add(dependBundle);
        }


        var sceneBundle = AssetBundle.LoadFromFile(Const.kPersistentABDir + sceneBundleName);
        if (sceneBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        _sceneBundleList.AddRange(dependBundleList.ToArray());
        _sceneBundleList.Add(sceneBundle);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // 场景的话，这里的bundle不能unload,unload就有问题了
        //for (int i = 0; i < dependBundleList.Count; i++)
        //{
        //    dependBundleList[i].Unload(false);
        //}
        //sceneBundle.Unload(false);
    }


    private void OnDestroy()
    {
        _rootBundle.Unload(true);
    }

    void UnLoadSceneBundle()
    {
        for (int i = 0; i < _sceneBundleList.Count; i++)
        {
            _sceneBundleList[i].Unload(false);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("***** Scene " + level);
        // 在这里添加unload assetBundle是可以释放assetBundle
        //UnLoadBundle();
    }
}
