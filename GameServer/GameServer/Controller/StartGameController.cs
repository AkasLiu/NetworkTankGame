using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;
using GameServer.Model;
using Common;

namespace GameServer.Controller
{
    //收到开始自己的游戏请求
    //1，寻找房间  ，没空位则新建一个房间
    //2，加入该房间
    //3，创建角色
    class StartGameController : BaseController
    {
        public StartGameController()
        {
            ContollerId = (int)ProtocolID.StartGame;
        }

        public override void HandleRequest(byte[] data, Client client, Server server)
        {
            if(data == null)
            {
                Console.WriteLine("startGame error");
            }

            StartGameProtocol startGameProtocol = new StartGameProtocol();
            startGameProtocol.Decode(data);

            Room room = server.RoomList.Last();

            if (room == null || room.CurrentCount >= room.Max_count)
            {
                room = new Room(room.RoomID+1);
                PlayerData playerData = new PlayerData(startGameProtocol.Role_Id, startGameProtocol.Mytf);
                client.playerData = playerData;
                client.RoomID = room.RoomID;
                room.clientList.Add(client);
                room.CurrentCount++;
                server.RoomList.Add(room);

                //新的房间只有自己，所有一个就够了
                JoinGameProtocol joinGameProtocol = new JoinGameProtocol(startGameProtocol.Role_Id, startGameProtocol.Mytf);
                byte[] stream = joinGameProtocol.Encode();
                client.Send(data);

            }
            else
            {
                PlayerData playerData = new PlayerData(startGameProtocol.Role_Id, startGameProtocol.Mytf);
                client.playerData = playerData;
                client.RoomID = room.RoomID;
                room.clientList.Add(client);
                room.CurrentCount++;

                //房间中有很多人    
                //1，更新当前客户端的服务器位置信息
                //2，将新的玩家坦克同步到其他客户端
                //3，自己的客户端生成其他敌人坦克

                foreach(Client c in room.clientList)
                {
                    JoinGameProtocol joinGameProtocol = new JoinGameProtocol(startGameProtocol.Role_Id, startGameProtocol.Mytf);
                    c.Send(joinGameProtocol.Encode());
                }

                foreach (Client c in room.clientList)
                {
                    if (c.playerData.Id != client.playerData.Id)
                    {
                        JoinGameProtocol joinGameProtocol = new JoinGameProtocol(c.playerData.Id,c.playerData.Mytf);
                        client.Send(joinGameProtocol.Encode());
                    }                   
                }

            }


            //1，更新当前客户端的服务器位置信息
            //2，将新的玩家坦克同步到其他客户端
            //3，自己的客户端生成其他敌人坦克
            //client.playerData.Id = startGameProtocol.Role_Id;
            //client.playerData.Mytf = startGameProtocol.Mytf;

            //JoinGameProtocol joinGameProtocol = new JoinGameProtocol(startGameProtocol.Role_Id, startGameProtocol.Mytf);
            //byte[] stream = joinGameProtocol.Encode();

            //client.Send(data);

            //foreach (Client c in server.ClientList)
            //{
            //    if (c != client)
            //    {
            //        Console.WriteLine("cc " + c.playerData.Id);
            //        server.SendResponse(stream, c);
            //        client.Send(new JoinGameProtocol(c.playerData.Id, c.playerData.Mytf).Encode());
            //    }               
            //}



        }
    }
}
