using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class SyncPositionProtocol : BaseProtocol
    {
        public MyTransform Stf { get; set; }
        public int Role_Id { get; set; }

        public SyncPositionProtocol()
        {
            Protocol_id = (int)ProtocolID.SyncPosition;
            Stf = new MyTransform();
        }

        public SyncPositionProtocol(int role_id, float x, float y, float z, float rx, float ry, float rz)
        {
            Protocol_id = (int)ProtocolID.SyncPosition;
            Role_Id = role_id;
            Stf = new MyTransform(x, y, z, rx, ry, rz);
        }

        public SyncPositionProtocol(int role_id, MyTransform stf)
        {
            Protocol_id = (int)ProtocolID.SyncPosition;
            Role_Id = role_id;
            this.Stf = stf;
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00000001 00000001 00000001 00000001 00000001 00000001 
        /// 总长度 + Pro_id + role_+id +x+y+z+rx+ry+rz
        /// </summary>
        /// <param name="data"></param>
        public override void Decode(byte[] data)
        {
            Role_Id = BitConverter.ToInt32(data, 8);
            Stf.X = BitConverter.ToSingle(data, 12);
            Stf.Y = BitConverter.ToSingle(data, 16);
            Stf.Z = BitConverter.ToSingle(data, 20);
            Stf.RX = BitConverter.ToSingle(data, 24);
            Stf.RY = BitConverter.ToSingle(data, 28);
            Stf.RZ = BitConverter.ToSingle(data, 32);
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00000001 00000001 00000001 00000001 00000001 00000001 
        /// 总长度 + Pro_id + role_+id +x+y+z+rx+ry+rz
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            int dataLength = 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4;

                List<byte> streamList = new List<byte>();

                streamList.AddRange(BitConverter.GetBytes(dataLength));
                streamList.AddRange(BitConverter.GetBytes(Protocol_id));
                streamList.AddRange(BitConverter.GetBytes(Role_Id));
            streamList.AddRange(BitConverter.GetBytes(Stf.X));
            streamList.AddRange(BitConverter.GetBytes(Stf.Y));
            streamList.AddRange(BitConverter.GetBytes(Stf.Z));
            streamList.AddRange(BitConverter.GetBytes(Stf.RX));
            streamList.AddRange(BitConverter.GetBytes(Stf.RY));
            streamList.AddRange(BitConverter.GetBytes(Stf.RZ));

            return streamList.ToArray();
        }
    }
}
