using UnityEngine;
using System.Collections;
using UnityEditor;

public class BuildBundle
{

    private const string VARIANT = "ab";
    [MenuItem("Test/Build Asset Bundles")]
    static void BuildABs()
    {
        // Create the array of bundle build details.  
        AssetBundleBuild[] buildMap = new AssetBundleBuild[3];

        /*
        // prfab  
        string[] assetNames = new string[1];
        assetNames[0] = "Assets/res/Prefabs/char/311009/311009.prefab";
        buildMap[0].assetNames = assetNames;
        buildMap[0].assetBundleName = "111";

        //texture  
        assetNames = new string[1];
        assetNames[0] = "Assets/res/Models/char/311009/Materials/311009_001.tga";
        buildMap[1].assetNames = assetNames;
        buildMap[1].assetBundleName = "222";

        //Mesh
        assetNames = new string[1];
        assetNames[0] = "Assets/res/Models/char/311009/311009.FBX";
        buildMap[2].assetNames = assetNames;
        buildMap[2].assetBundleName = "333";
        */

        
        // scene
        string[] assetNames = new string[1];
        assetNames[0] = "Assets/res/Scenes/1001/1001.unity";
        buildMap[0].assetNames = assetNames;
        buildMap[0].assetBundleName = "1001_Scene";

        assetNames = new string[1];
        assetNames[0] = "Assets/res/Scenes/1001_Test/SceneRoot.prefab";
        buildMap[1].assetNames = assetNames;
        buildMap[1].assetBundleName = "SceneRoot";
        

        string outputPath = Application.streamingAssetsPath + "/ABs";
        if (!System.IO.Directory.Exists(outputPath))
        {
            System.IO.Directory.CreateDirectory(outputPath);
        }
        

        BuildAssetBundleOptions op = BuildAssetBundleOptions.ChunkBasedCompression; // LZ4  

        BuildPipeline.BuildAssetBundles(outputPath, buildMap,op, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }
}