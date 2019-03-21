using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;
using Common;

namespace GameServer.Controller
{
    class StartGameController : BaseController
    {
        public StartGameController()
        {
            ContollerId = (int)ProtocolId.StartGame;
        }

        public override byte[] HandleRequest(byte[] data, Client client, Server server)
        {
            if(data == null)
            {
                Console.WriteLine("error");
            }

            StartGameProtocol startGameProtocol = new StartGameProtocol();
            startGameProtocol.Decode(data);

            //1，更新当前客户端的服务器位置信息
            //2，将新的玩家坦克同步到其他客户端
            //3，自己的客户端生成其他敌人坦克
            client.playerData.Id = startGameProtocol.Role_Id;
            client.playerData.stf = startGameProtocol.CTF;

            JoinGameProtocol joinGameProtocol = new JoinGameProtocol(startGameProtocol.Role_Id, startGameProtocol.CTF);
            byte[] stream = joinGameProtocol.Encode();

            client.Send(data);

            foreach (Client c in server.ClientList)
            {
                if (c != client)
                {
                    Console.WriteLine("cc " + c.playerData.Id);
                    server.SendResponse(stream, c);
                    client.Send(new JoinGameProtocol(c.playerData.Id, c.playerData.stf).Encode());
                }               
            }

            return null;

        }
    }
}
