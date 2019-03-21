using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    //创建了服务器，实现与客户端连接
    class Server
    {
        private IPEndPoint iPEndPoint;
        private Socket serverSocket;
        private ControllerManager controllerManager;
        private List<Client> clientList;

        public Server() { }
        public Server(string ipStr, int port)
        {
            SetIpAndPort(ipStr, port);
            clientList = new List<Client>();
            controllerManager = new ControllerManager(this);
        }

        public List<Client> ClientList
        {
            get { return clientList; }
        }

        public void SetIpAndPort(string ipStr, int port)
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        //启动
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallBack,null);

            Console.WriteLine("服务器开启成功......");
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket,this);
            clientList.Add(client);
            client.Start();

            Console.WriteLine("一个客户端连接到服务器......");

            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        /// <summary>
        /// 服务器处理客户端发来的请求,将请求转发给controller层
        /// 被client类调用
        /// </summary>
        /// <param name="protocol_Id"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public void HandleRequest(int protocol_Id, byte[] data, Client client)
        {
            controllerManager.HandleRequest(protocol_Id, data, client, this);
        }

        /// <summary>
        /// 服务器向客户端发送响应，让客户端对应的clientsocket执行发送命令
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void SendResponse(byte[] data, Client client)
        {
            //Console.WriteLine("对客户端发起响应");
            if (data != null)
            {
                client.Send(data);
            }
            
        }


    }
}
