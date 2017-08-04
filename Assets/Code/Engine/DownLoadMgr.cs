using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadMgr : MonoBehaviour {

    private bool _isDownLoadFinish = false;
    // Use this for initialization
    void Start()
    {
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
			Const.kStreamABDir = "file://" + Application.streamingAssetsPath + "/ABs/";
		} else if (Application.platform == RuntimePlatform.Android) {

			//Note that on some platforms it is not possible to directly access the StreamingAssets folder 
			//because there is no file system access in the web platforms, 
			//and because it is compressed into the .apk file on Android. 
			//On those platforms, a url will be returned, which can be used using the WWW class.

			Const.kStreamABDir = "jar:file://" + Application.dataPath + "!/assets" + "/ABs/";

		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			
			Const.kStreamABDir = "file://" + Application.dataPath + "/Raw" + "/ABs/";
		}



		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
			Const.kPersistentABDir = Application.persistentDataPath + "/ABs/";
		} else if (Application.platform == RuntimePlatform.Android) {
			Const.kPersistentABDir = Application.persistentDataPath + "/ABs/";
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			Const.kPersistentABDir = Application.persistentDataPath + "/ABs/";
		}

        ProcessDownload();
    }

    void ProcessDownload()
    {
        string[] srcUrlList = new string[]
        {
            "1001",
            "1001.manifest",
            "ABs",
            "ABs.manifest",
            "sky",
            "sky.manifest",
            "306025",
            "306025.manifest",
            "306025_001_FBX",
            "306025_001_FBX.manifest",
            "306025_001_mat",
            "306025_001_mat.manifest",
            "306025_001_tga",
            "306025_001_tga.manifest"

        };

        for (int i = 0; i < srcUrlList.Length; i++)
        {
            string srcPath = Const.kStreamABDir + srcUrlList[i];
            string dstPath = Const.kPersistentABDir + srcUrlList[i];
            StartCoroutine(DoDownload(srcPath, dstPath, i, srcUrlList.Length));
        }

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

            if (index == (maxIndex - 1))
            {
                _isDownLoadFinish = true;

                InitLoadManager();
            }
        }
        www.Dispose();
    }

    void WriteFile(string path, byte[] content)
    {
        
        string dirPath = System.IO.Path.GetDirectoryName(path);
        if (!System.IO.Directory.Exists(dirPath)) System.IO.Directory.CreateDirectory(dirPath);
        System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
        fs.Write(content, 0, content.Length);
        Debug.Log("成功下载 : " + path);
        fs.Close();
    }

    void InitLoadManager()
    {
        GameObject loadSceneGo = new GameObject("LoadSceneMgr");
        loadSceneGo.AddComponent<LoadSceneMgr>();
    }

    void OnGUI()
    {
        if (_isDownLoadFinish)
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Next"))
            {
                LoadSceneMgr loadSceneMgr = GameObject.FindObjectOfType<LoadSceneMgr>();
                loadSceneMgr.LoadScene("1001", "1001");
                //loadSceneMgr.LoadSceneAsync("1001", "1001.ab");

                //UnityEngine.SceneManagement.SceneManager.LoadScene("empty");
            }
        }

    }
}
