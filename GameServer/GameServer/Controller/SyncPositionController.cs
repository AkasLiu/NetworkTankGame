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
        public SyncPositionController()
        {
            ContollerId = (int)ProtocolId.SyncPosition;
        }

        public override byte[] HandleRequest(byte[] data, Client client, Server server)
        {
            SyncPositionProtocol syncPositionProtocol = new SyncPositionProtocol();
            syncPositionProtocol.Decode(data);

            //1，更新当前客户端的服务器位置信息
            //2,将当前位置广播给所有客户端
            client.playerData.stf = syncPositionProtocol.Stf;
       
            foreach (Client c in server.ClientList)
            {
                if (c != client)
                {
                    server.SendResponse(data, c);
                }
            }

            return null;

        }
    }
}
