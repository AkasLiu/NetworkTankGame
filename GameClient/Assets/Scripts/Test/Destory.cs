using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(de());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator de()
    {
        yield return new WaitForSeconds(5);
        Destroy(this);
    }
}
