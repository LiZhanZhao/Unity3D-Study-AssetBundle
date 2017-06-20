using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneAB : MonoBehaviour {

    AssetBundle _rootBundle;
    AssetBundleManifest _manifest;
    string kStreamABDir;
    string kPersistentABDir;
    List<AssetBundle> _bundleList = new List<AssetBundle>();
    bool _isInitManifest = false;

    void InitManifest()
    {
        string mainManifestPath = kPersistentABDir + "ABs";
        _rootBundle = AssetBundle.LoadFromFile(mainManifestPath);
        _manifest = _rootBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        GameObject.DontDestroyOnLoad(gameObject);

        _isInitManifest = true;
    }

    private void OnDestroy()
    {
        _rootBundle.Unload(true);
    }

    // 直接读取streamingAssets目录
    void ChangeScene()
    {
        LoadScene("1001", "1001_Scene");
    }

    void LoadScene(string sceneName, string sceneBundleName)
    {
        List<AssetBundle> dependBundleList = new List<AssetBundle>();
        string[] dependPath = _manifest.GetAllDependencies(sceneBundleName);
        for (int i = 0; i < dependPath.Length; i++)
        {
            Debug.Log(dependPath[i]);
            var dependBundle = AssetBundle.LoadFromFile(kPersistentABDir + dependPath[i]);
            dependBundleList.Add(dependBundle);
        }
        

        var sceneBundle = AssetBundle.LoadFromFile(kPersistentABDir + sceneBundleName);
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
        //UnLoadSceneBundle();
    }

    
    IEnumerator DoDownload(string srcUrl, string dstUrl, int index, int maxIndex)
    {
        WWW www = new WWW(srcUrl);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(string.Format("下载 {0} 失败", srcUrl));
        }
        else
        {
            WriteFile(dstUrl, www.bytes);

            if(index == (maxIndex - 1))
            {
                InitManifest();
            }
        }
        www.Dispose();
    }

    void WriteFile(string path, byte[] content)
    {
        string dirPath = System.IO.Path.GetDirectoryName(path);
        Debug.Log("-------- " + dirPath);
        if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);
        System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
        fs.Write(content, 0, content.Length);
        fs.Close();
    }

    void ProcessDownload()
    {
        string[] srcUrlList = new string[]
        {
            "1001_scene",
            "1001_scene.manifest",
            "ABs",
            "ABs.manifest",
        };

        for (int i = 0; i < srcUrlList.Length; i++)
        {
            string srcPath = kStreamABDir + srcUrlList[i];
            string dstPath = kPersistentABDir + srcUrlList[i];
            StartCoroutine(DoDownload(srcPath, dstPath, i, srcUrlList.Length));
        }

    }

    void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            kStreamABDir = "file://" + Application.streamingAssetsPath + "/ABs/";
        }
        
        else if(Application.platform == RuntimePlatform.Android){

            //Note that on some platforms it is not possible to directly access the StreamingAssets folder 
            //because there is no file system access in the web platforms, 
            //and because it is compressed into the .apk file on Android. 
            //On those platforms, a url will be returned, which can be used using the WWW class.

            kStreamABDir = "jar:file://" + Application.dataPath + "!/assets" + "/ABs/";
        }




        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            kPersistentABDir = Application.persistentDataPath + "/ABs/";
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            kPersistentABDir = Application.persistentDataPath + "/ABs/";
        }

            
        ProcessDownload();
    }

    void OnGUI()
    {
        if (_isInitManifest)
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Next"))
            {
                ChangeScene();
            }
        }
        
    }


    /*
    private static string GetAssetsPath(string relaPath = "")
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.dataPath + "/Raw/" + relaPath;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            return "jar:file://" + Application.dataPath + "!/assets/" + relaPath;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            return Application.dataPath + "/StreamingAssets/" + relaPath;
        }
        else
        {
            return Application.dataPath + "/" + relaPath;
        }
    }
     */
}
