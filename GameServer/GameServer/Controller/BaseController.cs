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
            ContollerId = (int)ProtocolId.None;
        }

        public virtual byte[] HandleRequest(byte[] data, Client client, Server server) { return null; }
        //public virtual void DefaultHandle();
    }
}
