using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;

namespace GameServer.Controller
{
    class ReviveController : BaseController
    {
        public ReviveController()
        {
            ContollerId = (int)ProtocolId.Revive;
        }

        public override byte[] HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client, server);

            ReviveProtocol reviveProtocol = new ReviveProtocol();
            reviveProtocol.Decode(data);

            foreach (Client c in server.ClientList)
            {
                server.SendResponse(data, c); 
            }

            return null;
        }
    }
}
