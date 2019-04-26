using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ExitGameController : BaseController
    {
        public ExitGameController()
        {
            ContollerId = (int)ProtocolID.ExitGame;
        }

        public override void HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client, server);

            foreach (Client c in server.clientsInRoom(client.RoomID))
            {
                if(c != client)
                {                    
                    server.SendResponse(data, c);
                }
            }

            server.clientsInRoom(client.RoomID).Remove(client);
            server.FindRoomById(client.RoomID).CurrentCount--;
            client.RoomID = -1;
            //移除房间
        }
    }
}
