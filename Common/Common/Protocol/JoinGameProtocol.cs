using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    //生成坦克
    public class JoinGameProtocol : BaseProtocol
    {
        public MyTransform mytf { get; set; }
        public int Role_Id { get; set; }

        public JoinGameProtocol()
        {
            Protocol_id = (int)ProtocolID.JoinGame;
            mytf = new MyTransform();
        }

        public JoinGameProtocol(int role_id, float x, float y, float z, float rx, float ry, float rz)
        {
            Protocol_id = (int)ProtocolID.JoinGame;
            mytf = new MyTransform(x, y, z, rx, ry, rz);
            Role_Id = role_id;
        }

        public JoinGameProtocol(int role_id, MyTransform mytf)
        {
            Protocol_id = (int)ProtocolID.JoinGame;
            this.mytf = mytf;
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
            Console.WriteLine(mytf + " Stf");
            mytf.X = BitConverter.ToSingle(data, 12);
            mytf.Y = BitConverter.ToSingle(data, 16);
            mytf.Z = BitConverter.ToSingle(data, 20);
            mytf.RX = BitConverter.ToSingle(data, 24);
            mytf.RY = BitConverter.ToSingle(data, 28);
            mytf.RZ = BitConverter.ToSingle(data, 32);
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
            streamList.AddRange(BitConverter.GetBytes(mytf.X));
            streamList.AddRange(BitConverter.GetBytes(mytf.Y));
            streamList.AddRange(BitConverter.GetBytes(mytf.Z));
            streamList.AddRange(BitConverter.GetBytes(mytf.RX));
            streamList.AddRange(BitConverter.GetBytes(mytf.RY));
            streamList.AddRange(BitConverter.GetBytes(mytf.RZ));

            return streamList.ToArray();
        }
    }
}
