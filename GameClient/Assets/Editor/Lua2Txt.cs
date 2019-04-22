using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class Lua2Txt {

    [MenuItem("LuatoTxt/lua2txt")]
    static void LuaToTxt()
    {
        string[] files = Directory.GetFiles("Assets/HotFix/Lua", "*.lua", SearchOption.AllDirectories);

        string dir = "Assets/HotFix/Out";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }

        for (int i = 0; i < files.Length; i++)
        {
            Debug.Log(files[i]);
            string fname = Path.GetFileName(files[i]);
            FileUtil.CopyFileOrDirectory(files[i], "Assets/HotFix/Out/" + fname + ".bytes");
        }

    }
}
