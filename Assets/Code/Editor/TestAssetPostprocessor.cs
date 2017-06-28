
using UnityEngine;
using UnityEditor;
using System.IO;
using System;



class TestAssetPostprocessor : AssetPostprocessor {
	
	void OnPostprocessModel(GameObject model){
        
       
        /*
        Renderer[] renderComs = model.GetComponentsInChildren<Renderer>();
        Debug.Log("---- " + renderComs.Length);
        for (int i = 0; i < renderComs.Length; i++)
        {
            renderComs[i].sharedMaterial = null;

            if (renderComs[i].sharedMaterials != null)
            {
                renderComs[i].sharedMaterials = new Material[0];
            }

            //for(int j = 0; j < renderComs[i].sharedMaterials.Length; j++)
            //{
            //    Debug.Log("++++ " + renderComs[i].sharedMaterials.Length);
            //    Material mat = renderComs[i].sharedMaterials[j];
            //    Debug.Log("**** " + mat.shader.name);

            //}
        }
        */
    }

   
}