using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInspector : MonoBehaviour {
  
    public int id;
    public string username;

    public Vector3 vector3;

    Tank tank;

	// Use this for initialization
	void Start () {
        


	}
	
	// Update is called once per frame
	void Update () {
        id = PlayerInfoManager.Instance.GetUserData().ID;
        username = PlayerInfoManager.Instance.GetUserData().Username;
    }
}
