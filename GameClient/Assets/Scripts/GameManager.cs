using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;
using System.IO;

public class GameManager : MonoBehaviour {

    LuaSvr svr;
    LuaTable self;
    LuaFunction update;

    [CustomLuaClass]
    public delegate void UpdateDelegate(object self);

    UpdateDelegate ud;

    void Start()
    {
        gameObject.AddComponent<HotFixManager>();


        NetworkManager.Instance.Start();

        

        //svr = new LuaSvr();
        //LuaSvr.mainState.loaderDelegate += LuaFileLoader;
        //svr.init(null, () =>
        //{
        //    self = (LuaTable)svr.start("Entry");
        //    update = (LuaFunction)self["update"];
        //    ud = update.cast<UpdateDelegate>();
        //});

    }

    void Update()
    {
        if (ud != null)
            ud(self);
    }

    void FixedUpdate()
    {
        NetworkManager.Instance.FixedUpdate();
    }

    void OnDestroy()
    {
       // NetworkManager.Instance.OnDestroy();
    }

    // 加载lua文件Delagate
    private byte[] LuaFileLoader(string strFile, ref string absoluteFn)
    {
        if (strFile == null)
        {
            return null;
        }
        //string filename = Application.dataPath + "/Resources/Lua/" + strFile.Replace('.', '/') + ".lua";
        string filename = Application.streamingAssetsPath + "/Resources/Lua/" + strFile.Replace('.', '/') + ".lua";
        return File.ReadAllBytes(filename);
    }
}
