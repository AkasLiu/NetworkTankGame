using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    void Start()
    {
        NetworkManager.Instance.Start();
        UIManager.Instance.Start();
        StateMachine.Instance.Start();
        //SceneManager.Instance.Start();
    }

    void Update()
    {
        NetworkManager.Instance.Update();
        UIManager.Instance.Update();
    }

    void OnDestroy()
    {
        NetworkManager.Instance.OnDestroy();
    }
}
