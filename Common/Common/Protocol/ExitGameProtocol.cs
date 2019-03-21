using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class ExitGameProtocol : BaseProtocol
    {
        public int Role_id { get; set; }

        public ExitGameProtocol()
        {
            Protocol_id = (int)ProtocolId.ExitGame;
        }

        public ExitGameProtocol(int role_Id)
        {
            Protocol_id = (int)ProtocolId.ExitGame;
            Role_id = role_Id;
        }

        /// <summary>
        /// 总长度 + protoId + roleId 
        /// </summary>
        /// <param name="data"></param>
        public override void Decode(byte[] data)
        {
            Role_id = BitConverter.ToInt32(data, 8);
        }

        public override byte[] Encode()
        {
            int dataLength = 4 + 4 + 4;

            List<byte> streamList = new List<byte>();

            streamList.AddRange(BitConverter.GetBytes(dataLength));
            streamList.AddRange(BitConverter.GetBytes(Protocol_id));
            streamList.AddRange(BitConverter.GetBytes(Role_id));

            return streamList.ToArray();
        }
    }
}
