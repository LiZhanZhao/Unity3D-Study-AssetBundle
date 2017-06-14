using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneAB : MonoBehaviour {

    // 直接读取streamingAssets目录
    void ChangeScene()
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

        string sceneName = "1001";
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);


        // 场景的话，这里的bundle不能unload,unload就有问题了
        //for (int i = 0; i < dependBundleList.Count; i++)
        //{
        //    dependBundleList[i].Unload(false);
        //}
        //myLoadedAssetBundle.Unload(false);
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
