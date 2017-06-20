using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneMgr : MonoBehaviour {

    AssetBundle _rootBundle;
    AssetBundleManifest _manifest;
    //List<AssetBundle> _sceneBundleList = new List<AssetBundle>();
    Dictionary<string, AssetBundle> _bundleCache = new Dictionary<string, AssetBundle>();

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
        var sceneBundle = LoadBundle(sceneBundleName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // 场景的话，这里的bundle不能unload,unload就有问题了
        //for (int i = 0; i < dependBundleList.Count; i++)
        //{
        //    dependBundleList[i].Unload(false);
        //}
        //sceneBundle.Unload(false);
    }

    //public void LoadSceneAsync(string sceneName, string sceneBundleName)
    //{
    //    List<AssetBundle> dependBundleList = new List<AssetBundle>();
    //    string[] dependPath = _manifest.GetAllDependencies(sceneBundleName);
    //    for (int i = 0; i < dependPath.Length; i++)
    //    {
    //        Debug.Log(dependPath[i]);
    //        StartCoroutine(LoadBundleAsync(Const.kPersistentABDir + dependPath[i]));
    //    }
    //}

    AssetBundle LoadBundle(string bundleName)
    {
        if (_bundleCache.ContainsKey(bundleName))
        {
            return _bundleCache[bundleName];
        }

        string[] dependBundleNames = _manifest.GetAllDependencies(bundleName);
        for (int i = 0; i < dependBundleNames.Length; i++)
        {
            string dependBundleName = dependBundleNames[i];
            Debug.Log(dependBundleName);
            var dependBundle = LoadBundle(dependBundleName);
        }

        string bundlePath = Const.kPersistentABDir + bundleName;
        var bundle = AssetBundle.LoadFromFile(bundlePath);
        Debug.Assert(bundle != null, string.Format("加载 {0} bundle 失败", bundlePath));
        _bundleCache.Add(bundleName, bundle);
        return bundle;
    }


    //IEnumerator LoadBundleAsync(string path)
    //{
    //    AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(path);
    //    yield return bundleRequest;
    //    var bundle = bundleRequest.assetBundle;
    //    if (bundle == null)
    //    {
    //        Debug.LogError(string.Format("Failed to load {0} AssetBundle!", path));
    //        yield break;
    //    }

        
    //}


    private void OnDestroy()
    {
        _rootBundle.Unload(true);
    }

    void UnLoadBundleCache()
    {
        foreach(KeyValuePair<string, AssetBundle> item in _bundleCache)
        {
            item.Value.Unload(false);
        }
        _bundleCache.Clear();
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("***** Scene " + level);
        // 在这里添加unload assetBundle是可以释放assetBundle
        //UnLoadBundleCache();
    }
}
