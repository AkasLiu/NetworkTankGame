using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HotFixManager : MonoBehaviour
{
    GameObject hotfixView;
    Slider progressSilder;
    Text hotFixTip;
    string server_version;
    string client_version;
    string versionFileName = "version.txt";
    float cunrrentProgress  = 0.0f ;

    Dictionary<string, string> server_bundle_list_Dict = new Dictionary<string, string>();
    Dictionary<string, string> client_bundle_list_Dict = new Dictionary<string, string>();
    Dictionary<string, string> new_bundle_list_Dict = new Dictionary<string, string>();
    public bool isFinished = false;

    string filePath;

    // Use this for initialization
    public void Start()
    {
        hotfixView = Resources.Load("Prefabs/UI/HotFixView") as GameObject;
        hotfixView = GameObject.Instantiate(hotfixView);
        progressSilder = GameObject.Find("ProgressSilder").GetComponent<Slider>();
        hotFixTip = GameObject.Find("HotFixTip").GetComponent<Text>();

        filePath = Application.persistentDataPath;

#if UNITY_ANDROID && !UNITY_EDITOR
        //第一次进入 
        //将streamingAssets资源全部复制到persistentDataPath
        if (!File.Exists(Application.persistentDataPath + @"/version.txt"))
        {
            Debug.Log("the first");

            StartCoroutine(CopyToLocal());        

        }
        else
        {
            //1,先下载version.txt与本地进行比较 2,下载服务器的bundle_list 和 客户端的对比
            Debug.Log("not the first");
            StartCoroutine(CheckVersion());
        }

#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (!File.Exists(filePath + @"/version.txt"))
        {
            Debug.Log("the first");            

            DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] fis = di.GetFiles();
            foreach (FileInfo fi in fis)
            {
                string destPath = filePath + @"/" + fi.Name;
                File.Copy(fi.FullName, destPath);
                cunrrentProgress++;
                hotFixTip.text = string.Format("首次解压 当前进度 {0}/{1}", cunrrentProgress, fis.Length);
                progressSilder.value = cunrrentProgress / fis.Length;
            }

            Debug.Log("start download");

            StartCoroutine(CheckVersion());
           
        }
        else
        {
            //1,先下载version.txt与本地进行比较 2,下载服务器的bundle_list 和 客户端的对比
            Debug.Log("not the first");
            StartCoroutine(CheckVersion());

        }
#endif

    }


    //使用webreuqest 试试？
    IEnumerator CopyToLocal()
    {
        WWW www = new WWW(Application.streamingAssetsPath + @"/version.txt");
        yield return www;
        if (www.isDone)
        {
            string path = Application.persistentDataPath + @"/version.txt";
            File.WriteAllBytes(path, www.bytes);
        }
        www.Dispose();

        www = new WWW(Application.streamingAssetsPath + @"/bundle_list.txt");
        yield return www;
        if (www.isDone)
        {
            string path = Application.persistentDataPath + @"/bundle_list.txt";
            File.WriteAllBytes(path, www.bytes);
        }
        www.Dispose();

        string local_bundle_list = ReadClientContent(Application.persistentDataPath + @"/bundle_list.txt");
        string[] local_assset_crc_list = local_bundle_list.Split('-', '\n');
        for (int i = 0; i < local_assset_crc_list.Length - 1; i = i + 2)
        {
            www = new WWW(Application.streamingAssetsPath + @"/" + local_assset_crc_list[i]);
            yield return www;
            if (www.isDone)
            {
                string path = Application.persistentDataPath + @"/" + local_assset_crc_list[i];
                File.WriteAllBytes(path, www.bytes);

                cunrrentProgress++;
                hotFixTip.text = string.Format("首次解压 当前进度 {0}/{1}", cunrrentProgress, local_assset_crc_list.Length/2);
                progressSilder.value = cunrrentProgress / (local_assset_crc_list.Length / 2);
                Debug.Log(cunrrentProgress / (local_assset_crc_list.Length / 2));
                yield return new WaitForSeconds(1f);
            }            
            www.Dispose();           
        }
        cunrrentProgress = 0;

        //检查版本
        StartCoroutine(CheckVersion());
    }


    IEnumerator CheckVersion()
    {
        string uri = @"http://192.168.191.1/StreamingAssets/version.txt";
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;
        server_version = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        client_version = ReadClientContent(filePath + @"/version.txt");
        Debug.Log(server_version);
        Debug.Log(client_version);

        if (server_version != client_version)
        {
            Debug.Log("热更");
            Dictionary<string, string> newBundleList = new Dictionary<string, string>();
            StartCoroutine(CheckBundleList());
        }
        else
        {
            Debug.Log("版本一致，不需要更新");
            isFinished = true;
        }
    }

    IEnumerator CheckBundleList()
    {
        string uri = @"http://192.168.191.1/StreamingAssets/bundle_list.txt";
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;
        string server_bundle_list = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        string[] server_assset_crc_list = server_bundle_list.Split('-','\n');
        for (int i=0;i< server_assset_crc_list.Length-1; i=i+2)
        {
            server_bundle_list_Dict.Add(server_assset_crc_list[i], server_assset_crc_list[i+1]);
        }
        
        string client_bundle_list = ReadClientContent(filePath + @"/bundle_list.txt");
        string[] client_assset_crc_list = client_bundle_list.Split('-', '\n');
        for (int i = 0; i < client_assset_crc_list.Length-1; i = i + 2)
        {
            client_bundle_list_Dict.Add(client_assset_crc_list[i], client_assset_crc_list[i + 1]);
        }

        cunrrentProgress = 0;
        //遍历服务器字典
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

            Debug.Log("c "+cunrrentProgress);
            Debug.Log(server_bundle_list_Dict.Count);

            cunrrentProgress++;
            hotFixTip.text = string.Format("热更进度 当前进度 {0}%", cunrrentProgress/server_bundle_list_Dict.Count*100);
            progressSilder.value = cunrrentProgress / server_bundle_list_Dict.Count;
            yield return new WaitForSeconds(1f);
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
        string uri = @"http://192.168.191.1/StreamingAssets/" + name;
        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;

        Debug.Log(uri);

        Stream sw = null;
        FileInfo t = new FileInfo(filePath + @"/" + name);
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

    IEnumerator AlertVersionAndBundleList(string version_name, string bundle_name)
    {
        string version_uri = @"http://192.168.191.1/StreamingAssets/" + version_name;
        string bundle_uri = @"http://192.168.191.1/StreamingAssets/" + bundle_name;

        UnityWebRequest request = UnityWebRequest.Get(version_uri);
        yield return request.SendWebRequest();
        AlertFile(filePath + @"/" + version_name, request.downloadHandler.data);
        request.Dispose();

        request = UnityWebRequest.Get(bundle_uri);
        yield return request.SendWebRequest();
        AlertFile(filePath + @"/" + bundle_name, request.downloadHandler.data);
        request.Dispose();       

        isFinished = true;

    }

    void AlertFile(string path, byte[] bytes)
    {
        Stream sw = null;
        FileInfo fi = new FileInfo(path);
        if (!fi.Exists)
        {
            sw = fi.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
        }
        else
        {
            fi.Delete();
            sw = fi.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
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

    private void OnDestroy()
    {
        Destroy(hotfixView);
    }
}
