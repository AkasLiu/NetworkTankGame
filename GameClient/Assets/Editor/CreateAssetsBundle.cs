using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class CreateAssetsBundle : MonoBehaviour {

    private readonly static string version = "1.3.9";

    //打两种包  先将lua变为bytes 然后再变回lua
    //假如StreamingAssets存在，则删除里面所有文件，生成ab包
    [MenuItem("BuildeAssetBundel/Windows64")]
    static void BuildeAssetBundlesWin64()
    {
        LuaToBytes();
        AssetDatabase.Refresh();

        string dir = @"Assets\StreamingAssets";

        if (Directory.Exists(dir))
        {
            foreach (string s in Directory.GetFiles(dir))
            {
                File.Delete(s);
            }
            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
        else
        {

            Directory.CreateDirectory(dir);
            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }

        CreateCRCList();
        CreateVsersion();

        BytesToLua();
    }

    [MenuItem("BuildeAssetBundel/Android")]
    static void BuildeAssetBundlesAndroid()
    {
        LuaToBytes();
        AssetDatabase.Refresh();

        string dir = @"Assets\StreamingAssets";

        if (Directory.Exists(dir))
        {
            foreach (string s in Directory.GetFiles(dir))
            {
                File.Delete(s);
            }
            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.Android);
        }
        else
        {
            Directory.CreateDirectory(dir);
            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.Android);
        }

        CreateCRCList();
        CreateVsersion();

        BytesToLua();
    }

    //创建CRC     格式: 路径+CRC
    static void CreateCRCList()
    {       
        string bundle_list_path = @"Assets\StreamingAssets\bundle_list.txt";
        DirectoryInfo di = new DirectoryInfo(@"Assets\StreamingAssets");
        FileInfo[] fis = di.GetFiles("*.unity3d");
        FileStream fs = new FileStream(bundle_list_path, FileMode.Create);
        StreamWriter wr = new StreamWriter(fs);
        try
        {
            foreach (FileInfo fi in fis)
            {
                byte[] buff = new byte[fi.Length];
                FileStream fsbuf = fi.OpenRead();
                fsbuf.Read(buff, 0, Convert.ToInt32(fsbuf.Length));
                fsbuf.Close();

                uint crc = CRC32.GetCRC32(buff);
                wr.WriteLine(fi.Name + "-" + crc);
            }
            wr.Close();
            fs.Close();
        }
        catch(Exception e)
        {
            Debug.Log(e);
            return;
        }
        
    }

    //创建版本号
    static void CreateVsersion()
    { 
        string versionPath = @"Assets\StreamingAssets\version.txt";
        try
        {
            FileStream fs = new FileStream(versionPath, FileMode.Create);
            StreamWriter wr = new StreamWriter(fs);
            wr.WriteLine(version);
            wr.Close();
            fs.Close();
        }
        catch(Exception e)
        {
            Debug.Log(e);
            return;
        }
        
    }

    //[MenuItem("LuaAndTxt/lua2txt")]
    static void LuaToBytes()
    {
        string[] files = Directory.GetFiles(@"Assets\Resources\HotFix\Lua", "*.lua", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            File.Move(files[i], files[i].Replace(".lua", ".bytes"));
        }
    }

    //[MenuItem("LuaAndTxt/txt2lua")]
    static void BytesToLua()
    {
        string[] files = Directory.GetFiles(@"Assets\Resources\HotFix\Lua", "*.bytes", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            File.Move(files[i], files[i].Replace(".bytes", ".lua"));
        }
    }

}
