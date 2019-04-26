using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class ResultProtocol : BaseProtocol
    {
        public bool Result { get; set; }

        public ResultProtocol()
        {
            Protocol_id = (int)ProtocolID.ReturnResult;
        }

        public ResultProtocol(bool result)
        {
            Protocol_id = (int)ProtocolID.ReturnResult;
            Result = result;
        }

        public override void Decode(byte[] data)
        {
            int dataLength = BitConverter.ToInt32(data, 0);
            int protocolId = BitConverter.ToInt32(data, 4);
            Result = BitConverter.ToBoolean(data, 8);
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00
        /// 总长度+id+值
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            byte[] resultBytes = BitConverter.GetBytes(Result);
            int resultBytesLength = resultBytes.Length;

            int data_overallLength = resultBytesLength + 4 + 4;

            List<byte> streamList = new List<byte>();

            streamList.AddRange(BitConverter.GetBytes(data_overallLength));
            streamList.AddRange(BitConverter.GetBytes(Protocol_id));
            streamList.AddRange(resultBytes);

            return streamList.ToArray();
        }
    }
}
