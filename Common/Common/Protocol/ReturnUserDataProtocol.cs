using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class ReturnUserDataProtocol : BaseProtocol
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public ReturnUserDataProtocol()
        {
            Protocol_id = (int)ProtocolID.ReturnUserDataProtocol;
        }

        public ReturnUserDataProtocol(int id, string username)
        {
            Protocol_id = (int)ProtocolID.ReturnUserDataProtocol;
            this.Id = id;
            this.Username = username;
        }

        /// <summary>
        // 编码格式
        /// 总长度+id+ role_id+长度+username
        /// </summary>
        /// <param name="data"></param>
        public override void Decode(byte[] data)
        {
            this.Id = BitConverter.ToInt32(data, 8);
            int usernameLength = BitConverter.ToInt32(data, 12);
            this.Username = Encoding.UTF8.GetString(data, 16, usernameLength);
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00
        /// 总长度+id+ role_id+长度+username
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {
            List<byte> streamList = new List<byte>();
            int data_overallLength;

            byte[] idBytes = BitConverter.GetBytes(Id);
            byte[] usernameBytes = Encoding.UTF8.GetBytes(Username);

            data_overallLength = 4 + 4 + 4 + 4 + usernameBytes.Length;

            streamList.AddRange(BitConverter.GetBytes(data_overallLength));
            streamList.AddRange(BitConverter.GetBytes(Protocol_id));
            streamList.AddRange(BitConverter.GetBytes(Id));
            streamList.AddRange(BitConverter.GetBytes(usernameBytes.Length));
            streamList.AddRange(usernameBytes);
           
            return streamList.ToArray();
        }
    }
}
