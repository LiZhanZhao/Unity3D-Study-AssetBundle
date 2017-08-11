using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneMgr : MonoBehaviour {

    AssetBundle _rootBundle;
    AssetBundleManifest _manifest;
    //List<AssetBundle> _sceneBundleList = new List<AssetBundle>();
    Dictionary<string, AssetBundle> _bundleCache = new Dictionary<string, AssetBundle>();
    Dictionary<string, bool> _lockBundles = new Dictionary<string, bool>();
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
        LoadBundle(sceneBundleName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // 场景的话，这里的bundle不能unload,unload就有问题了
        //for (int i = 0; i < dependBundleList.Count; i++)
        //{
        //    dependBundleList[i].Unload(false);
        //}
        //sceneBundle.Unload(false);
    }

    

    public AssetBundle LoadBundle(string bundleName)
    {
        if (!_bundleCache.ContainsKey(bundleName))
        {
            string[] dependBundleNames = _manifest.GetAllDependencies(bundleName);

            //Debug.Log(string.Format("*********************** {0} : {1}", bundleName, dependBundleNames.Length));
            for (int i = 0; i < dependBundleNames.Length; i++)
            {
                string dependBundleName = dependBundleNames[i];
                //Debug.Log(string.Format("dependBundleName : {1}", i, dependBundleName));
            }

            for (int i = 0; i < dependBundleNames.Length; i++)
            {
                string dependBundleName = dependBundleNames[i];
                LoadBundle(dependBundleName);
            }

            string bundlePath = Const.kPersistentABDir + bundleName;
            var bundle = AssetBundle.LoadFromFile(bundlePath);
            Debug.Assert(bundle != null, string.Format("加载 {0} bundle 失败", bundlePath));
            Debug.Log(string.Format("{0} 同步 加载成功", bundleName));
            _bundleCache.Add(bundleName, bundle);
        }
        return _bundleCache[bundleName];
    }

    public void LoadSceneAsync(string sceneName, string sceneBundleName)
    {
        StartCoroutine(DoLoadBundleAsync(sceneBundleName));
        StartCoroutine(DoLoadSceneAsync(sceneName, sceneBundleName));
    }

    IEnumerator DoLoadSceneAsync(string sceneName, string sceneBundleName)
    {
        while (!_bundleCache.ContainsKey(sceneBundleName))
        {
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    void LockBundle(string bundleName)
    {
        if (!_lockBundles.ContainsKey(bundleName))
        {
            _lockBundles.Add(bundleName, true);
        }
    }

    void UnLockBundle(string bundleName)
    {
        if (_lockBundles.ContainsKey(bundleName))
        {
            _lockBundles.Remove(bundleName);
        }
    }

    bool IsLockBundle(string bundleName)
    {
        return _lockBundles.ContainsKey(bundleName);
    }

    bool IsFinishLoadDependBundle(string[] dependBundleNames)
    {
        bool isFinish = true;
        for(int i = 0; i < dependBundleNames.Length; i++)
        {
            if (!_bundleCache.ContainsKey(dependBundleNames[i]))
            {
                isFinish = false;
            }
        }
        return isFinish;
    }

    IEnumerator DoLoadBundleAsync(string bundleName)
    { 
        if (!_bundleCache.ContainsKey(bundleName) || !IsLockBundle(bundleName))
        {
            // 锁住了某一个Bundle
            LockBundle(bundleName);

            string[] dependBundleNames = _manifest.GetAllDependencies(bundleName);
            for (int i = 0; i < dependBundleNames.Length; i++)
            {
                string dependBundleName = dependBundleNames[i];
                StartCoroutine(DoLoadBundleAsync(dependBundleName));
            }

            // 如果依赖Bundle的还没有好的话，就进行等待
            while (!IsFinishLoadDependBundle(dependBundleNames))
            {
                yield return null;
            }

            string bundlePath = Const.kPersistentABDir + bundleName;
            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return bundleRequest;
            var bundle = bundleRequest.assetBundle;
            if (bundle == null)
            {
                Debug.LogError(string.Format("LoadBundleAsync {0} AssetBundle faild", bundlePath));
                yield break;
            }
            Debug.Log(string.Format("{0} 异步 加载成功", bundleName));

            _bundleCache.Add(bundleName, bundle);

            UnLockBundle(bundleName);

        }

    }



    private void OnDestroy()
    {
        _rootBundle.Unload(true);
    }

    public void UnLoadBundleCache()
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
        UnLoadBundleCache();
    }
}
