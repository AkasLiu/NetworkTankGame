﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;

namespace GameServer.Servers
{
    //与客户端通信
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        //private User user;
        public PlayerData playerData = new PlayerData();

        private byte[] data = new byte[1024];        

        //public bool IsDie()
        //{
        //    return HP <= 0;
        //}

        public MySqlConnection MySQLConn { get; }

        public Client() { }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            MySQLConn = ConnHelper.Connect();

        }

        //public int GetUserId()
        //{
        //    return user.Id;
        //}

        //public string GetUsername()
        //{
        //    return user.Username;
        //}

        //public void GetPlayerData

        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false)
                return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        //边接收边处理
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();

                }
                //TODO 处理接收的数据
                msg.ReadMessage(count,OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        private void OnProcessMessage(int protocol_Id, byte[] data)
        {
            server.HandleRequest(protocol_Id, data, this);
        }

        public void Send(byte[] data)
        {            
            clientSocket.Send(data);
        }

        private void Close()
        {
            if (clientSocket != null)
            {
                ConnHelper.DisConnect(MySQLConn);
                if (clientSocket != null)
                {
                    Console.WriteLine("有一个客户端断开连接");
                    clientSocket.Close();
                }
                server.RemoveClient(this);
            }
        }
    }
}