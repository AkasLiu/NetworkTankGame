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
    class LoginController : BaseController
    {
        public LoginController()
        {
            ContollerId = (int)ProtocolId.Login;
        }

        public override byte[] HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client,server);
            UserDAO userDAO = new UserDAO();
            LoginProtocol loginProtocol = new LoginProtocol();
            loginProtocol.Decode(data);           
            User user = userDAO.VerifyUser(client.MySQLConn, loginProtocol.Username, loginProtocol.Password);
            if (user == null)
            {
                Console.WriteLine("登陆失败");
                ReturnUserDataProtocol returnUserDataProtocol = new ReturnUserDataProtocol(false);
                return returnUserDataProtocol.Encode();
            }
            else
            {
                Console.WriteLine("登陆成功");  
                client.playerData.Id = user.Id;
                client.playerData.Username = user.Username;
                ReturnUserDataProtocol returnUserDataProtocol = new ReturnUserDataProtocol(true, user.Id, user.Username);
                return returnUserDataProtocol.Encode();
            }
        }
    }
}
