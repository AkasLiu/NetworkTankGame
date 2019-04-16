using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Protocol;
using Common;

// Controller
public class SceneManager : Singleton<SceneManager>
{
    //List<Tank> _tanks - new List<Tank>();

    //yaobuyaodangqian  tank
    Material gold;
    Dictionary<int, Tank> tanksDict;
    GameObject tankPrefab;
    GameObject shellPrefab;
    GameObject envPrefab;
    GameObject tankExplosionPrefab;

    GameObject env;
    GameObject tankGameObject;
    GameObject shellGameObject;
    
    


    public SceneManager()
    {
        tanksDict = new Dictionary<int, Tank>();
        tankPrefab = Resources.Load("Prefabs/Tank") as GameObject;
        shellPrefab = Resources.Load("Prefabs/Shell") as GameObject;
        envPrefab = Resources.Load("Prefabs/Env") as GameObject;
        tankExplosionPrefab = Resources.Load("Prefabs/TankExplosion") as GameObject;
        gold = Resources.Load("Materials/Gold") as Material;
    }

    //进入场景， 注册协议
    public void Start()
    {
        env = Object.Instantiate(envPrefab, Vector3.zero, Quaternion.identity);

        NetworkManager.Instance.RegisterProto((int)ProtocolId.SyncPosition, SyncPosition);
        NetworkManager.Instance.RegisterProto((int)ProtocolId.Fire, FireShell);
        NetworkManager.Instance.RegisterProto((int)ProtocolId.Die, Die);
        NetworkManager.Instance.RegisterProto((int)ProtocolId.Revive, Revive);

    }

    //退出场景 ,摧毁所有物体，然后反注册
    public void Destory()
    {
        Camera.main.transform.SetParent(null);

        if(tanksDict !=null)
        {
            foreach (KeyValuePair<int, Tank> kvp in tanksDict)
            {
                if (kvp.Value.SelfGameObject != null)
                {
                    Object.DestroyImmediate(kvp.Value.SelfGameObject);
                    kvp.Value.SelfGameObject = null;
                }               
            }
            tanksDict.Clear();
        }
        Object.Destroy(env);

        //todo 退出游戏协议

        NetworkManager.Instance.UnregisterProto((int)ProtocolId.SyncPosition, SyncPosition);
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.Fire, FireShell);
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.Die, Die);
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.Revive, Revive);

    }

    //1，将坦克根据位置生成 2，将数据加进两个字典中 3，假如创建的是自己的游戏物体，则加入摄像机并修改自身材质,
    public void GenerateTank(int id, CustomTransform customTransform)
    {
        if (FindTanksById(id) == null)
        {
            Vector3 position = new Vector3(customTransform.X, customTransform.Y, customTransform.Z);
            Vector3 rotationVec = new Vector3(customTransform.RX, customTransform.RY, customTransform.RZ);
            tankGameObject = Object.Instantiate(tankPrefab, new Vector3(customTransform.X, customTransform.Y, customTransform.Z), Quaternion.Euler(rotationVec));

            Tank tank = new Tank(id, tankGameObject);
            tanksDict.Add(id, tank);

            if (PlayerInfoManager.Instance.GetUserData().ID == id)
            {
                Camera.main.transform.parent = tankGameObject.transform;
                Camera.main.transform.localPosition = new Vector3(0.0f, 5.0f, -8.0f);
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
                Camera.main.transform.Rotate(20, 0, 0, Space.Self);

                MeshRenderer[] meshRenderers = tankGameObject.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    meshRenderer.material = gold;
                }
            }
        }        

    }

    public void OnReceiveMove(int userID, float hor, float ver)
    {
        // 操作 userID 指定的坦克，让它移动到 x,y
        Vector3 direction = new Vector3(hor, 0, ver);

        GameObject tempTank = FindTanksById(userID).SelfGameObject;

        if (direction != Vector3.zero)
        {
            tempTank.transform.Translate(tempTank.transform.forward * ver / 10 * Time.deltaTime, Space.World);
            tempTank.transform.transform.Rotate(0, hor / 2 * Time.deltaTime, 0, Space.Self);
        }

        //SyncPositionProtocol syncPositionProtocol = new SyncPositionProtocol(PlayerInfoManager.Instance.GetUserData().ID,
        //   tempTank.transform.position.x,
        //    tempTank.transform.position.y,
        //    tempTank.transform.position.z,
        //    tempTank.transform.localEulerAngles.x,
        //    tempTank.transform.localEulerAngles.y,
        //    tempTank.transform.localEulerAngles.z
        //    );

        //NetworkManager.Instance.Send(syncPositionProtocol);

    }

    public void SyncPosition(object param)
    {
        byte[] data = (byte[])param;
        SyncPositionProtocol syncPositionProtocol = new SyncPositionProtocol();
        syncPositionProtocol.Decode(data);

        int role_id = syncPositionProtocol.Role_Id;
        CustomTransform stf = syncPositionProtocol.Stf;

        GameObject tempTank = FindTanksById(role_id).SelfGameObject;
        //tempTank.transform.position = Vector3.Lerp(tempTank.transform.position, new Vector3(stf.X, stf.Y, stf.Z), Time.deltaTime);
        tempTank.transform.position = new Vector3(stf.X, stf.Y, stf.Z);
        tempTank.transform.Rotate(0, stf.RY - tempTank.transform.localEulerAngles.y, 0, Space.Self);
    }

    public void FireShell(object param)
    {
        byte[] data = (byte[])param;
        FireProtocol fireProtocol = new FireProtocol();
        fireProtocol.Decode(data);
        GameObject firingTank = FindTanksById(fireProtocol.Role_id).SelfGameObject;
        Transform firePosition = firingTank.transform.Find("FirePosition");
        GameObject shell = Object.Instantiate(shellPrefab, firePosition.position, firePosition.rotation);
        shell.AddComponent<Shell>();
        shell.GetComponent<Rigidbody>().AddForce(firingTank.transform.forward * 1000);
    }

    //1,先让摄像机移除将要摧毁的tank 让died物体爆炸，然后返回出生点，  如果需要 可能要注册和反注册
    public void Die(object param)
    {
        byte[] data = (byte[])param;
        DieProtocol dieProtocol = new DieProtocol();
        dieProtocol.Decode(data);
       
        if (dieProtocol.Role_id == PlayerInfoManager.Instance.GetUserData().ID)
        {
            Camera.main.transform.SetParent(null);
            ReviveProtocol reviveProtocol = new ReviveProtocol(PlayerInfoManager.Instance.GetUserData().ID);
            NetworkManager.Instance.Send(reviveProtocol);
        }

        if (FindTanksById(dieProtocol.Role_id) != null)
        {
            GameObject gameObject = FindTanksById(dieProtocol.Role_id).SelfGameObject;            
            GameObject tankExplosion = Object.Instantiate(tankExplosionPrefab, gameObject.transform);
            Object.Destroy(tankExplosion, 1);
            Object.DestroyImmediate(gameObject);
            tanksDict.Remove(dieProtocol.Role_id);
        }             
    }
    
    public void Revive(object param)
    {
        byte[] data = (byte[])param;
        ReviveProtocol reviveProtocol = new ReviveProtocol();
        reviveProtocol.Decode(data);

        CustomTransform customTransform = new CustomTransform(9.0f, -1.0f, -7.0f, 0.0f, 0.0f, 0.0f);

        GenerateTank(reviveProtocol.Role_id, customTransform);
    }





    #region 工具方法

    public Tank FindTanksById(int id)
    {
        Tank tempTank;
        tanksDict.TryGetValue(id, out tempTank);
        return tempTank;
    }

    public int FindIdByTanks(GameObject Go)
    {
        foreach (KeyValuePair<int, Tank> kvp in tanksDict)
        {
            if (kvp.Value.SelfGameObject.Equals(Go))
            {
                return kvp.Key;
            }
        }

        return -1;
    }

    public void DebugTank()
    {
        Debug.Log("new tankInfo");
        //foreach (KeyValuePair<int, Tank> kvp in tanksInfo)
        //{
        //    Debug.Log("id " + kvp.Key + " Tank " + kvp.Value);
        //}
    }

    #endregion



}
