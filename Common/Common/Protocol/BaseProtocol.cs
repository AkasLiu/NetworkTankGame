using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    /// <summary>
    /// protocol基类
    /// </summary>
    public abstract class BaseProtocol
    {
        public BaseProtocol()
        {
            Protocol_id = (int)ProtocolID.None;
        }

        public int Protocol_id { get; set; }      

        public abstract byte[] Encode();

        public abstract void Decode(byte[] data);
 
        public int DecodeProtoId(byte[] data)
        {
            return BitConverter.ToInt32(data,4);
        }

        public int ProtocolLength(byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }



        public byte[] HandleIntToBytes(int i)
        {
            byte[] stream = new byte[4];
            stream[0] = (byte)(i >> 24);
            stream[1] = (byte)(i >> 16);
            stream[2] = (byte)(i >> 8);
            stream[3] = (byte)(i);
            return stream;
        }

        public int HandleBytesToInt(byte[] bytes)
        {
            int x = bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
            return x;
        }

    }
}
