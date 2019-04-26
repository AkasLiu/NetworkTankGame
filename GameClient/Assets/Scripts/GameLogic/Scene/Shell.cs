using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Protocol;
using SLua;

[CustomLuaClass]
public class Shell : MonoBehaviour {

    [CustomLuaClass]
    public delegate void DiePrtocolSendDelg(GameObject collidedObject);
    public DiePrtocolSendDelg dpsd;

    [CustomLuaClass]
    public delegate void TestPrint();
    public TestPrint tp;

    GameObject shellExplosionPrefab;

    private void Awake()
    {
        shellExplosionPrefab = Resources.Load("Prefabs/ShellExplosion") as GameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);

        GameObject shellExplosion = Instantiate(shellExplosionPrefab, this.gameObject.transform.position, Quaternion.identity);

        if (collision.gameObject.name == "Tank(Clone)")
        {
            dpsd(collision.gameObject);
        }

        Destroy(this.gameObject);
        Destroy(shellExplosion, 1);

    }

    public void ADD(LuaFunction function)
    {
        TestPrint test = function.cast<TestPrint>();
    }
}
