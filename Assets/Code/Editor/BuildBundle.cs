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

        // 在Ios上，assetBundleName和 assetNames 名字要一致，不然无法读取assetbundle
        // scene
        string[] assetNames = new string[1];
        assetNames[0] = "Assets/res/Scenes/1001/1001.unity";
        buildMap[0].assetNames = assetNames;
        buildMap[0].assetBundleName = "1001";
		buildMap[0].assetBundleVariant = "ab";


        assetNames = new string[1];
        assetNames[0] = "Assets/res/Models/scene/1001/FBX/sky.FBX";
        buildMap[1].assetNames = assetNames;
        buildMap[1].assetBundleName = "sky";
		buildMap[1].assetBundleVariant = "ab";



        string outputPath = Application.streamingAssetsPath + "/ABs";
        if (!System.IO.Directory.Exists(outputPath))
        {
            System.IO.Directory.CreateDirectory(outputPath);
        }
        

		BuildAssetBundleOptions op = BuildAssetBundleOptions.ChunkBasedCompression; // LZ4  BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
		//BuildAssetBundleOptions op = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;;

        //打Pc assetBundle
        BuildPipeline.BuildAssetBundles(outputPath, buildMap,op, BuildTarget.StandaloneWindows);

        // 打android assetBundle
        //BuildPipeline.BuildAssetBundles(outputPath, buildMap, op, BuildTarget.Android);

		// 打iOS assetBundle
		//BuildPipeline.BuildAssetBundles(outputPath, buildMap, op, BuildTarget.iOS);

        AssetDatabase.Refresh();
    }
}