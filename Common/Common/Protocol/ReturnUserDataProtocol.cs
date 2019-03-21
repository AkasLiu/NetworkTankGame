using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class ReturnUserDataProtocol : BaseProtocol
    {
        public bool Result { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }

        public ReturnUserDataProtocol()
        {
            Protocol_id = (int)ProtocolId.ReturnUserDataProtocol;
            Result = false;
        }

        public ReturnUserDataProtocol(bool result)
        {
            Protocol_id = (int)ProtocolId.ReturnUserDataProtocol;
            Result = result;
        }

        public ReturnUserDataProtocol(bool result, int id, string username)
        {
            Protocol_id = (int)ProtocolId.ReturnUserDataProtocol;
            Result = result;
            this.Id = id;
            this.Username = username;
        }
        /// <summary>
        // 编码格式
        /// 0x 00000014 00000001 00
        /// 总长度+id+结果+ role_id+长度+username
        /// </summary>
        /// <param name="data"></param>
        public override void Decode(byte[] data)
        {
            bool res = BitConverter.ToBoolean(data,8);
            Result = res;
            if (res)
            {
                this.Id = BitConverter.ToInt32(data, 9);
                int usernameLength = BitConverter.ToInt32(data, 13);
                this.Username = Encoding.UTF8.GetString(data, 17, usernameLength);
            }
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00
        /// 总长度+id+结果+ role_id+长度+username
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            List<byte> streamList = new List<byte>();

            byte[] resultBytes = BitConverter.GetBytes(Result);
            int data_overallLength;

            if (Result)
            {
                byte[] idBytes = BitConverter.GetBytes(Id);
                byte[] usernameBytes = Encoding.UTF8.GetBytes(Username);

                data_overallLength = 4 + 4 + 1 + 4 + 4 +usernameBytes.Length;

                streamList.AddRange(BitConverter.GetBytes(data_overallLength));
                streamList.AddRange(BitConverter.GetBytes(Protocol_id));
                streamList.AddRange(resultBytes);
                streamList.AddRange(BitConverter.GetBytes(Id));
                streamList.AddRange(BitConverter.GetBytes(usernameBytes.Length));
                streamList.AddRange(usernameBytes);
            }
            else
            {
                data_overallLength = 4 + 4 + 1;

                streamList.AddRange(BitConverter.GetBytes(data_overallLength));
                streamList.AddRange(BitConverter.GetBytes(Protocol_id));
                streamList.AddRange(resultBytes);                
            }
            return streamList.ToArray();
        }
    }
}
