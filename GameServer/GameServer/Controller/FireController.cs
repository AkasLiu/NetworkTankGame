﻿using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;

namespace GameServer.Controller
{
    class FireController : BaseController
    {
        public FireController()
        {
            ContollerId = (int)ProtocolId.Fire;
        }

        public override byte[] HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client, server);

            FireProtocol fireProtocol = new FireProtocol();
            fireProtocol.Decode(data);

            foreach (Client c in server.ClientList)
            {
                server.SendResponse(data, c);
            }

            return null;
        }
    }
}
