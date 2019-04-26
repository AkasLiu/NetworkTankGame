using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;
using GameServer.Servers;

namespace GameServer.Controller
{
    class SyncPositionController : BaseController
    {
        //long timeStamp = 0;

        public SyncPositionController()
        {
            ContollerId = (int)ProtocolID.SyncPosition;
        }

        public override void HandleRequest(byte[] data, Client client, Server server)
        {

            //long a = DateTime.Now.Ticks / 10000 - timeStamp;
            //timeStamp = DateTime.Now.Ticks / 10000;
            //if (a > 50)
            //{
            //    Console.WriteLine(a);
            //}

            SyncPositionProtocol syncPositionProtocol = new SyncPositionProtocol();
            syncPositionProtocol.Decode(data);

            //1，更新当前客户端的服务器位置信息
            //2,将当前位置广播给所有客户端
            //client.playerData.Mytf = syncPositionProtocol.Stf;

            foreach (Client c in server.clientsInRoom(client.RoomID))
            {
                if (c != client)
                {
                    server.SendResponse(data, c);
                }
            }

        }
    }
}
