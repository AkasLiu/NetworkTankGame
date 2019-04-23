using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HotFixManager : MonoBehaviour
{
    GameObject hotfixView;
    string server_version;
    string client_version;
    Dictionary<string, string> server_bundle_list_Dict = new Dictionary<string, string>();
    Dictionary<string, string> client_bundle_list_Dict = new Dictionary<string, string>();
    Dictionary<string, string> new_bundle_list_Dict = new Dictionary<string, string>();
    public bool isFinished = false;


    // Use this for initialization
    public void Start ()
    {
        hotfixView = Resources.Load("Prefabs/UI/HotFixView") as GameObject;
        hotfixView = GameObject.Instantiate(hotfixView);

        //第一次进入 streamingAssets到persistentDataPath
        if (!File.Exists(Application.persistentDataPath + @"/version.txt"))
        {
            Debug.Log("the first");
            Directory.CreateDirectory(Application.persistentDataPath);
            DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fis = di.GetFiles("*.unity3d");
            foreach (FileInfo fi in fis)
            {
                string destPath = Application.persistentDataPath + @"/"+fi.Name;
                File.Copy(fi.FullName, destPath);
            }

            FileInfo[] ffis = di.GetFiles("*.txt");
            foreach (FileInfo fi in ffis)
            {
                string destPath = Application.persistentDataPath + @"/" + fi.Name;
                File.Copy(fi.FullName, destPath);
            }
        }
        else
        {
            //1,先下载version.txt与本地进行比较 2,下载服务器的bundle_list 和 客户端的对比
            Debug.Log("not the first");
            StartCoroutine(DownloadVersion());
            
        }


    }

    private void Update()
    {
        if (isFinished)
        {
            gameObject.AddComponent<GameManager>();
            Destroy(this);            
        }
    }


    IEnumerator DownloadVersion()
    {
        string uri = @"http://localhost/StreamingAssets/version.txt";
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;
        server_version = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        client_version = ReadClientContent(Application.persistentDataPath + @"/version.txt");
        if (server_version != client_version)
        {
            Dictionary<string, string> newBundleList = new Dictionary<string, string>();
            StartCoroutine(DownloadBundleList());
        }
        else
        {
            //finished
        }

        isFinished = true;
    }

    IEnumerator DownloadBundleList()
    {
        string uri = @"http://localhost/StreamingAssets/bundle_list.txt";
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;

        string server_bundle_list = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        string[] server_assset_crc_list = server_bundle_list.Split('-','\n');
        foreach (string s in server_assset_crc_list)
            Debug.Log(s);
        for (int i=0;i< server_assset_crc_list.Length-1; i=i+2)
        {
            server_bundle_list_Dict.Add(server_assset_crc_list[i], server_assset_crc_list[i+1]);
        }
        

        string client_bundle_list = ReadClientContent(Application.persistentDataPath + @"/bundle_list.txt");
        string[] client_assset_crc_list = client_bundle_list.Split('-', '\n');
        foreach (string s in client_assset_crc_list)
            Debug.Log(s);
        for (int i = 0; i < client_assset_crc_list.Length-1; i = i + 2)
        {
            client_bundle_list_Dict.Add(client_assset_crc_list[i], client_assset_crc_list[i + 1]);
        }
        Debug.Log(client_assset_crc_list.ToString());

        //遍历服务器字典
        Debug.Log("bianlzidiain");
        foreach (KeyValuePair<string, string> kvp in server_bundle_list_Dict)
        {
            //假如没有这个资源，则下载
            if (!client_bundle_list_Dict.ContainsKey(kvp.Key))
            {
                StartCoroutine(SaveAB(kvp.Key));
            }
            else
            {
                //判断crc值是否相等
                if (server_bundle_list_Dict[kvp.Key] != client_bundle_list_Dict[kvp.Key])
                {
                    StartCoroutine(SaveAB(kvp.Key));
                }
            }

            //todo 不一样
        }

        //修改两个txt  todo
        StartCoroutine(AlertVersionAndBundleList("version.txt","bundle_list.txt"));
    }


    string ReadClientContent(string path)
    {
        FileStream stream = new FileInfo(path).OpenRead();
        var bufferLength = stream.Length;
        byte[] bufferFile = new byte[bufferLength];
        stream.Read(bufferFile, 0, Convert.ToInt32(bufferLength));
        string context = Encoding.UTF8.GetString(bufferFile, 0, bufferFile.Length);
        stream.Close();
        stream.Dispose();
        return context;
    }

    IEnumerator SaveAB(string name)
    {
        string uri = @"http://localhost/StreamingAssets/" + name;
        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;

        Debug.Log(uri);

        Stream sw = null;
        FileInfo t = new FileInfo(Application.persistentDataPath + @"/" + name);
        if (!t.Exists)
        {
            sw = t.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
        }
        else
        {
            t.Delete();
            sw = t.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
        }        
       
    }

    IEnumerator AlertVersionAndBundleList(string version_name,string bundle_name)
    {
        string version_uri = @"http://localhost/StreamingAssets/" + version_name;
        string bundle_uri = @"http://localhost/StreamingAssets/" + bundle_name;

        UnityWebRequest request = UnityWebRequest.Get(version_uri);

        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;

        Stream sw = null;
        FileInfo t_3 = new FileInfo(Application.persistentDataPath + @"/" + version_name);
        if (!t_3.Exists)
        {
            sw = t_3.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
        }
        else
        {
            t_3.Delete();
            sw = t_3.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
        }

        UnityWebRequest request_2 = UnityWebRequest.Get(bundle_uri);

        yield return request_2.SendWebRequest();
        byte[] bytes_2 = request_2.downloadHandler.data;

        Stream sw_2 = null;
        FileInfo t_2 = new FileInfo(Application.persistentDataPath + @"/" + bundle_name);
        if (!t_2.Exists)
        {
            sw_2 = t_2.Create();
            sw_2.Write(bytes_2, 0, bytes_2.Length);
            sw_2.Close();
            sw_2.Dispose();
        }
        else
        {
            t_2.Delete();
            sw_2 = t_2.Create();
            sw_2.Write(bytes_2, 0, bytes_2.Length);
            sw_2.Close();
            sw_2.Dispose();
        }

    }

}
