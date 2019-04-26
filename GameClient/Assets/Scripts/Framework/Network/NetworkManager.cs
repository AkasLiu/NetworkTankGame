using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Common.Protocol;
using System;
using SLua;

[CustomLuaClass]
public class NetworkManager : Singleton<NetworkManager>
{
    //Thread _socketThread; // 负责把协议接收好，放到协议队列

    float time = 0;

    private static string IP = "192.168.191.1";
    private static int Port = 6688;

    private Socket clientSocket;
    private Message msg = new Message();
    private Queue<MSG> msgQueue = new Queue<MSG>();

    public delegate void OnReceiveProtoDlg(object param);

    public new static NetworkManager Instance
    {
        get
        {
            return Singleton<NetworkManager>.Instance;
        }
    }


    // Dictionary for proto id to callback list
    Dictionary<int, List<OnReceiveProtoDlg>> _protoID2CallbackListDict = new Dictionary<int, List<OnReceiveProtoDlg>>();


    public void RegisterProto(int id, OnReceiveProtoDlg callback)
    {
        if (callback == null)
            return;

        if (!_protoID2CallbackListDict.ContainsKey(id))
            _protoID2CallbackListDict[id] = new List<OnReceiveProtoDlg>();

        _protoID2CallbackListDict[id].Add(callback);
    }

    public void UnregisterProto(int id, OnReceiveProtoDlg callback)
    {
        if (_protoID2CallbackListDict[id] != null)
            _protoID2CallbackListDict[id].Remove(callback);
    }

    /// <summary>
    ///start connection to server
    /// </summary>
    public void Start()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            clientSocket.Connect(IP, Port);
            Open();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法连接到服务器!" + e);
        }
    }

    /// <summary>
    /// 开始进行异步接受数据
    /// </summary>
    private void Open()
    {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    /// <summary>
    /// 开始接收数据
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            int count = clientSocket.EndReceive(ar);

            msg.ReadMessage(count, OnProcessDataCallback);

            Open();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //异步处理接受的数据
    private void OnProcessDataCallback(int protocol_Id, byte[] data)
    {
        msgQueue.Enqueue(new MSG(protocol_Id, data));
    }
    

    /// <summary>
    /// ？？？
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="proto"></param>
    public void Send(int pid, BaseProtocol proto)
    {
        byte[] data = proto.Encode();
        clientSocket.Send(data);
    }

    public void Send(BaseProtocol proto)
    {
        byte[] data = proto.Encode();

        //Debug.Log(Time.time- time);

        //time = Time.time;

        clientSocket.Send(data);
    }

    public void FixedUpdate()
    {
        while (true)
        {
            if (msgQueue.Count > 0)
            {
                lock (msgQueue)
                {
                    MSG msg = msgQueue.Dequeue();
                    Debug.Log(msg.Protocol_Id);
                    if (_protoID2CallbackListDict.ContainsKey(msg.Protocol_Id) && _protoID2CallbackListDict[msg.Protocol_Id] != null)
                    {
                        for (int i = 0; i < _protoID2CallbackListDict[msg.Protocol_Id].Count; ++i)
                        {
                            OnReceiveProtoDlg tempCallback = _protoID2CallbackListDict[msg.Protocol_Id][i];
                            tempCallback(msg.Data);
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
        
    }

    public void OnDestroy()
    {
        try
        {
            if (clientSocket != null)
            {
                // todo
                //if (StateMachine.Instance.GetCurrentState() == StateType.Battle)
                //{
                //    DieProtocol dieProtocol = new DieProtocol(PlayerInfoManager.Instance.GetUserData().ID);
                //    byte[] data = dieProtocol.Encode();
                //    clientSocket.Send(data);
                //}
                clientSocket.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("客户端无法关闭！" + e);
        }
    }

    public void SetIP(string s)
    {
        IP = s;
    }

    public void PrintTest()
    {
        Debug.Log("test NetManager");
    }


}
