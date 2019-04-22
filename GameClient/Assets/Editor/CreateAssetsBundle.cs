using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class CreateAssetsBundle : MonoBehaviour {

    [MenuItem("Assets/BuildeAssetBundel")]
    static void BuildeAssetBundles()
    {        
        string dir = "Assets/StreamingAssets";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);


        CreateCRCList();
        CreateVsersion();
    }

    //创建CRC     格式: 路径+CRC
    static void CreateCRCList()
    {
        FileStream fs = null;
        StreamWriter wr = null;
        
        string bundle_list_path = @"Assets/StreamingAssets/bundle_list.txt";
        DirectoryInfo di = new DirectoryInfo("Assets/StreamingAssets/");
        FileInfo[] fis = di.GetFiles("*.unity3d");
        fs = new FileStream(bundle_list_path, FileMode.Create);
        wr = new StreamWriter(fs);
        foreach (FileInfo fi in fis)
        {
            //print(fi.DirectoryName);

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

    //创建版本号
    static void CreateVsersion()
    {
        FileStream fs = null;
        StreamWriter wr = null;
      
        string versionPath = @"Assets/StreamingAssets/version.txt";
        string version = @"0.0.3";
        fs = new FileStream(versionPath, FileMode.Create);
        wr = new StreamWriter(fs);
        wr.WriteLine(version);       
        wr.Close();
        fs.Close();
    }

}
