using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Protocol;

public class Shell : MonoBehaviour {

    GameObject shellExplosionPrefab;

    private void Start()
    {
        shellExplosionPrefab = Resources.Load("Prefabs/ShellExplosion") as GameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        GameObject shellExplosion = Instantiate(shellExplosionPrefab, this.gameObject.transform.position, Quaternion.identity);

        if (collision.gameObject.name == "Tank(Clone)")
        {
            int role_id = SceneManager.Instance.FindIdByTanks(collision.gameObject);
            if (role_id != -1)
            {
                Debug.Log("id " + role_id);

                DieProtocol dieProtocol = new DieProtocol(role_id);
                NetworkManager.Instance.Send(dieProtocol);
            }
        }

        Destroy(this.gameObject);
        Destroy(shellExplosion, 1);

    }
}
