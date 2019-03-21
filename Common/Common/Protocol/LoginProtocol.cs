using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class LoginProtocol : BaseProtocol
    {
        public int Role_id { get; }
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginProtocol(string username, string password)
        {
            Protocol_id = (int)ProtocolId.Login;
            this.Username = username;
            this.Password = password;
        }

        public LoginProtocol()
        {
            Protocol_id = (int)ProtocolId.Login;
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00 00 00 03 23 23 34 00 00 00 02 23 24
        /// 总长度+id+长度+string+长度+string
        /// </summary>
        /// <param name="data"></param>
        public override void Decode(byte[] data)
        {
            int dataLength = BitConverter.ToInt32(data, 0);
            int protocolId = BitConverter.ToInt32(data, 4);
            int usernameBytesLength = BitConverter.ToInt32(data, 8);
            string username = Encoding.UTF8.GetString(data, 12, usernameBytesLength);
            int passwordBytesLength = BitConverter.ToInt32(data, 12 + usernameBytesLength);
            string password = Encoding.UTF8.GetString(data, 16 + usernameBytesLength, passwordBytesLength);

            Username = username;
            Password = password;
        }

        /// <summary>
        /// 编码格式
        /// 0x 00000014 00000001 00 00 00 03 23 23 34 00 00 00 02 23 24
        /// 总长度+id+长度+string+长度+string
        /// </summary>
        /// <returns></returns>
        public override byte[] Encode()
        {              
            byte[] usernameBytes = Encoding.UTF8.GetBytes(Username);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);

            int usernameBytesLength = usernameBytes.Length;
            int passwordBytesLength = passwordBytes.Length;
            int data_overallLength = usernameBytesLength + passwordBytesLength + 16;           

            List<byte> streamList = new List<byte>();

            streamList.AddRange(BitConverter.GetBytes(data_overallLength));
            streamList.AddRange(BitConverter.GetBytes(Protocol_id));
            streamList.AddRange(BitConverter.GetBytes(usernameBytesLength));
            streamList.AddRange(usernameBytes);
            streamList.AddRange(BitConverter.GetBytes(passwordBytesLength));
            streamList.AddRange(passwordBytes);

            return streamList.ToArray();
        }
    }
}
