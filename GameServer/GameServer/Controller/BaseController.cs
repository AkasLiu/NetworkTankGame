using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Controller
{
    class BaseController
    {
        public int ContollerId { get; set; }

        public BaseController()
        {
            ContollerId = (int)ProtocolID.None;
        }

        public virtual void HandleRequest(byte[] data, Client client, Server server) { }
    }
}
