using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class StartGameProtocol : BaseProtocol
    {
        public MyTransform Mytf { get; set; }
        public int Role_Id { get; set; }

        public StartGameProtocol()
        {
            Protocol_id = (int)ProtocolID.StartGame;
            Mytf = new MyTransform();
        }

        public StartGameProtocol(int role_id, float x, float y, float z, float rx, float ry, float rz)
        {
            Protocol_id = (int)ProtocolID.StartGame;
            Mytf = new MyTransform(x, y, z, rx, ry, rz);
            Role_Id = role_id;
        }

        public StartGameProtocol(int role_id, MyTransform mytf)
        {
            Protocol_id = (int)ProtocolID.StartGame;
            this.Mytf = mytf;
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
            Mytf.X = BitConverter.ToSingle(data, 12);
            Mytf.Y = BitConverter.ToSingle(data, 16);
            Mytf.Z = BitConverter.ToSingle(data, 20);
            Mytf.RX = BitConverter.ToSingle(data, 24);
            Mytf.RY = BitConverter.ToSingle(data, 28);
            Mytf.RZ = BitConverter.ToSingle(data, 32);
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
            streamList.AddRange(BitConverter.GetBytes(Mytf.X));
            streamList.AddRange(BitConverter.GetBytes(Mytf.Y));
            streamList.AddRange(BitConverter.GetBytes(Mytf.Z));
            streamList.AddRange(BitConverter.GetBytes(Mytf.RX));
            streamList.AddRange(BitConverter.GetBytes(Mytf.RY));
            streamList.AddRange(BitConverter.GetBytes(Mytf.RZ));

            return streamList.ToArray();
        }
    }
}
