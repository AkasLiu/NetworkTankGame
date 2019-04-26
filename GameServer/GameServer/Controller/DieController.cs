using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;

namespace GameServer.Controller
{
    class DieController : BaseController
    {
        public DieController()
        {
            ContollerId = (int)ProtocolID.Die;
        }

        public override void HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client, server);

            DieProtocol dieProtocol = new DieProtocol();
            dieProtocol.Decode(data);

            foreach (Client c in server.clientsInRoom(client.RoomID))
            {
                server.SendResponse(data, c);
            }

        }
    }
}
