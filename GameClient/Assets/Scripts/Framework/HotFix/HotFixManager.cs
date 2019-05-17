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

    string serverResouces_uri;
    string serverResoucresVersion_uri;
    string serverBundleList_uri;

    string server_version;
    string client_version;
    int cunrrentProgress = 0;

    Dictionary<string, string> server_bundle_list_Dict = new Dictionary<string, string>();
    Dictionary<string, string> client_bundle_list_Dict = new Dictionary<string, string>();
    public bool isFinished = false;

    // Use this for initialization
    public void Start()
    {
        hotfixView = Resources.Load("Prefabs/UI/HotFixView") as GameObject;
        hotfixView = GameObject.Instantiate(hotfixView);
        progressSilder = GameObject.Find("ProgressSilder").GetComponent<Slider>();
        hotFixTip = GameObject.Find("HotFixTip").GetComponent<Text>();


#if UNITY_ANDROID
        serverResouces_uri = "http://120.78.170.175/StreamingAssets/Android";
        serverResoucresVersion_uri = "http://120.78.170.175/StreamingAssets/Android/version.txt";
        serverBundleList_uri = "http://120.78.170.175/StreamingAssets/Android/bundle_list.txt";
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        serverResouces_uri = "http://120.78.170.175/StreamingAssets/Win/";
        serverResoucresVersion_uri = "http://120.78.170.175/StreamingAssets/Win/version.txt";
        serverBundleList_uri = "http://120.78.170.175/StreamingAssets/Win/bundle_list.txt";
#endif

        if (!File.Exists(Application.persistentDataPath + "/version.txt"))
        {
            Debug.Log("第一次进入游戏");
            StartCoroutine(CopyFileToLocal());
        }
        else
        {
            //1,先下载version.txt与本地进行比较 2,下载服务器的bundle_list 和 客户端的对比
            Debug.Log("not the first");
            StartCoroutine(CheckVersion());
        }


    }

    IEnumerator CopyFileToLocal()
    {

        WWW www = new WWW(Application.streamingAssetsPath + "/version.txt");
        Debug.Log(Application.streamingAssetsPath + "/version.txt");

        yield return www;
        if (www.isDone)
        {
            string path = Application.persistentDataPath + "/version.txt";
            File.WriteAllBytes(path, www.bytes);
        }
        www.Dispose();

        www = new WWW(Application.streamingAssetsPath + "/bundle_list.txt");
        yield return www;
        Debug.Log(www.text);
        if (www.isDone)
        {
            string path = Application.persistentDataPath + "/bundle_list.txt";
            File.WriteAllBytes(path, www.bytes);
        }
        www.Dispose();

        //error
        //UnityWebRequest request = new UnityWebRequest(Application.streamingAssetsPath + "/version.txt");
        //yield return request.SendWebRequest();
        //Debug.Log(request.downloadedBytes);
        //Content = request.downloadHandler.text;
        //File.WriteAllText(Application.persistentDataPath + "/version.txt", Content);
        //request.Dispose();

        //request = new UnityWebRequest(Application.streamingAssetsPath + "/bundle_list.txt");
        //yield return request.SendWebRequest();
        //Content = request.downloadHandler.text;
        //File.WriteAllText(Application.persistentDataPath + "/bundle_list.txt", Content);
        //request.Dispose();

        cunrrentProgress = 0;

        string local_bundle_list = ReadFileContent(Application.persistentDataPath + "/bundle_list.txt");
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
                hotFixTip.text = string.Format("首次解压 当前进度 {0}/{1}", cunrrentProgress, local_assset_crc_list.Length / 2);
                progressSilder.value = (float)cunrrentProgress / (local_assset_crc_list.Length / 2);
                yield return new WaitForSeconds(1f);
            }
            www.Dispose();
            Debug.Log(local_assset_crc_list[i]);
        }

        //检查版本
        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverResoucresVersion_uri);
        yield return request.SendWebRequest();
        server_version = request.downloadHandler.text;
        client_version = ReadFileContent(Application.persistentDataPath + "/version.txt");

        Debug.Log(server_version);
        Debug.Log(client_version);

        if (server_version != client_version)
        {
            Debug.Log("热更");
            Dictionary<string, string> newBundleList = new Dictionary<string, string>();
            yield return StartCoroutine(CheckBundleList());           
        }
        else
        {
            Debug.Log("版本一致，不需要更新");
            isFinished = true;
        }
    }

    IEnumerator CheckBundleList()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverBundleList_uri);
        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;
        string server_bundle_list = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        string[] server_asssetPath_crc_list = server_bundle_list.Split('-','\n');
        for (int i=0;i< server_asssetPath_crc_list.Length-1; i=i+2)
        {
            server_bundle_list_Dict.Add(server_asssetPath_crc_list[i], server_asssetPath_crc_list[i+1]);
        }
        
        string client_bundle_list = ReadFileContent(Application.persistentDataPath + "/bundle_list.txt");
        string[] client_asssetPath_crc_list = client_bundle_list.Split('-', '\n');
        for (int i = 0; i < client_asssetPath_crc_list.Length-1; i = i + 2)
        {
            client_bundle_list_Dict.Add(client_asssetPath_crc_list[i], client_asssetPath_crc_list[i + 1]);
        }

        cunrrentProgress = 0;

        //server_bundle_list_Dict与client_bundle_list_Dict对比crc值，不一样的则更新
        foreach (KeyValuePair<string, string> kvp in server_bundle_list_Dict)
        {
            //假如没有这个资源，则下载
            if (!client_bundle_list_Dict.ContainsKey(kvp.Key))
            {
                Debug.Log("download " + kvp.Key);
                yield return StartCoroutine(SaveAB(kvp.Key));
            }
            else
            {
                //判断crc值是否相等
                if (server_bundle_list_Dict.ContainsKey(kvp.Key) && server_bundle_list_Dict[kvp.Key] != client_bundle_list_Dict[kvp.Key])
                {
                    Debug.Log("download " + kvp.Key);
                    yield return StartCoroutine(SaveAB(kvp.Key));
                }
            }

            cunrrentProgress++;
            hotFixTip.text = string.Format("热更进度 当前进度 {0}%", ((float)cunrrentProgress /server_bundle_list_Dict.Count*100).ToString("#"));
            progressSilder.value = (float)cunrrentProgress / server_bundle_list_Dict.Count;
            yield return new WaitForSeconds(0.5f);
            //todo crc不一样
        }

        //修改两个txt
        yield return StartCoroutine(AlertVersionAndBundleList(Application.persistentDataPath + "/version.txt", Application.persistentDataPath + "/bundle_list.txt"));

        isFinished = true;
    }


    string ReadFileContent(string path)
    {
        StreamReader sr = new StreamReader(path);
        string content = sr.ReadToEnd();
        sr.Close();
        return content;
    }

    IEnumerator SaveAB(string name)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverResouces_uri+ "/"+ name);
        yield return request.SendWebRequest();
        byte[] bytes = request.downloadHandler.data;

        AlertFile(Application.persistentDataPath + "/" + name, request.downloadHandler.data); 

    }

    IEnumerator AlertVersionAndBundleList(string versionPath, string bundleListpath)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverResoucresVersion_uri);
        yield return request.SendWebRequest();
        AlertFile(versionPath, request.downloadHandler.data);
        request.Dispose();

        request = UnityWebRequest.Get(serverBundleList_uri);
        yield return request.SendWebRequest();
        AlertFile(bundleListpath, request.downloadHandler.data);
        request.Dispose();          
    }

    void AlertFile(string path, byte[] data)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        fs.Write(data, 0, data.Length);

        fs.Close();
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
