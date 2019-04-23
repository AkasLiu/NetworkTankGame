using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class Lua2Txt
{

    [MenuItem("LuaAndTxt/lua2txt")]
    static void LuaToTxt()
    {
        string[] files = Directory.GetFiles("Assets/HotFix/Lua/", "*.lua", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            File.Move(files[i], files[i].Replace(".lua",".bytes"));
        }
    }

    [MenuItem("LuaAndTxt/txt2lua")]
    static void TxtToLua()
    {
        string[] files = Directory.GetFiles("Assets/HotFix/Lua", "*.bytes", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            File.Move(files[i], files[i].Replace(".bytes", ".lua"));
        }
    }
}
