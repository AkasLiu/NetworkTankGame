using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class JoinGameProtocol : BaseProtocol
    {
        public CustomTransform CTF { get; set; }
        public int Role_Id { get; set; }

        public JoinGameProtocol()
        {
            Protocol_id = (int)ProtocolId.JoinGame;
            CTF = new CustomTransform();
        }

        public JoinGameProtocol(int role_id, float x, float y, float z, float rx, float ry, float rz)
        {
            Protocol_id = (int)ProtocolId.JoinGame;
            CTF = new CustomTransform(x, y, z, rx, ry, rz);
            Role_Id = role_id;
        }

        public JoinGameProtocol(int role_id, CustomTransform ctf)
        {
            Protocol_id = (int)ProtocolId.JoinGame;
            this.CTF = ctf;
            Role_Id = role_id;
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00000001 00000001 00000001 00000001 00000001 00000001 
        /// 总长度+id+ x+y+z+rx+ry+rz
        /// </summary>
        /// <param name="data"></param>
        public override void Decode(byte[] data)
        {
            Role_Id = BitConverter.ToInt32(data, 8);
            Console.WriteLine(Role_Id + " Role_Id");
            Console.WriteLine(CTF + " Stf");
            CTF.X = BitConverter.ToSingle(data, 12);
            CTF.Y = BitConverter.ToSingle(data, 16);
            CTF.Z = BitConverter.ToSingle(data, 20);
            CTF.RX = BitConverter.ToSingle(data, 24);
            CTF.RY = BitConverter.ToSingle(data, 28);
            CTF.RZ = BitConverter.ToSingle(data, 32);
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00000001 00000001 00000001 00000001 00000001 00000001 
        /// 总长度+id+ x+y+z+rx+ry+rz
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            int dataLength = 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4;

            List<byte> streamList = new List<byte>();

            streamList.AddRange(BitConverter.GetBytes(dataLength));
            streamList.AddRange(BitConverter.GetBytes(Protocol_id));
            streamList.AddRange(BitConverter.GetBytes(Role_Id));
            streamList.AddRange(BitConverter.GetBytes(CTF.X));
            streamList.AddRange(BitConverter.GetBytes(CTF.Y));
            streamList.AddRange(BitConverter.GetBytes(CTF.Z));
            streamList.AddRange(BitConverter.GetBytes(CTF.RX));
            streamList.AddRange(BitConverter.GetBytes(CTF.RY));
            streamList.AddRange(BitConverter.GetBytes(CTF.RZ));

            return streamList.ToArray();
        }
    }
}
