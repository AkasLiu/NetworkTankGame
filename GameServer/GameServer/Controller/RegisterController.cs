using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Servers;
using Common.Protocol;
using GameServer.DAO;
using GameServer.Model;

namespace GameServer.Controller
{
    class RegisterController : BaseController
    {
        public RegisterController()
        {
            ContollerId = (int)ProtocolID.Register;
        }

        public override void HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client, server);

            UserDAO userDAO = new UserDAO();
            RegisterProtocol registerProtocol = new RegisterProtocol();
            registerProtocol.Decode(data);
            User user = userDAO.VerifyUser(client.MySQLConn, registerProtocol.Username, registerProtocol.Password);         
         
            if (user == null)
            {
                Console.WriteLine("注册成功");

                userDAO.AddUser(client.MySQLConn, registerProtocol.Username, registerProtocol.Password);
                ResultProtocol resultProtocol = new ResultProtocol(true);
                server.SendResponse(resultProtocol.Encode(), client);
            }
            else
            {
                Console.WriteLine("注册失败");
                
                ResultProtocol resultProtocol = new ResultProtocol(false);
                server.SendResponse(resultProtocol.Encode(), client);
            }
        }
    }
}
