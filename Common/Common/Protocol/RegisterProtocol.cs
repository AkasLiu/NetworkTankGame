using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Protocol
{
    public class RegisterProtocol : BaseProtocol
    {
        public string Username { get; }
        public string Password { get; }

        public RegisterProtocol(string username, string password)
        {
            Protocol_id = 1002;
            this.Username = username;
            this.Password = password;
        }

        public override void Decode(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
