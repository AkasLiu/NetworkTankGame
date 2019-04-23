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
        NetworkManager.Instance.Start();

        svr = new LuaSvr();
        LuaSvr.mainState.loaderDelegate += LuaFileLoader;
        svr.init(null, () =>
        {
            self = (LuaTable)svr.start("Entry");
            update = (LuaFunction)self["update"];
            ud = update.cast<UpdateDelegate>();
        });

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
        //string filename = Application.persistentDataPath + "/Lua/Out/" + strFile.Replace('.', '/') + ".lua.bytes";
        Debug.Log(strFile);
        string filename = "assets/hotfix/lua/" + strFile.Replace('.', '/') + ".bytes";
        Debug.Log(filename);
        AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + @"/lua.unity3d");
        TextAsset textAsset = ab.LoadAsset<TextAsset>(filename) ;
        ab.Unload(false);
        //return File.ReadAllBytes(filename);
        return textAsset.bytes;
    }
}
