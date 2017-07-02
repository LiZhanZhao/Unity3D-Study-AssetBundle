using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyObj : MonoBehaviour {

    public GameObject saveGo = null;
    public Object saveAsset = null;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}

      
}
