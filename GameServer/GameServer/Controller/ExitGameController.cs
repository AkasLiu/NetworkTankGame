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
            ContollerId = (int)ProtocolId.ExitGame;
        }

        public override byte[] HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client, server);

            foreach (Client c in server.ClientList)
            {
                if(c != client)
                {
                    server.SendResponse(data, c);

                }
            }

            return null;
        }
    }
}
