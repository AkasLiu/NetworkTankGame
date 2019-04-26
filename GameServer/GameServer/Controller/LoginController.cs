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
    /// <summary>
    /// 登陆处理过程
    /// 1,收到客户端数据，解析出 username 和 password 与服务器对比
    /// 2，若正确则返回成功结果（ResultProtocol） 
    ///    若失败则返回失败结果（ResultProtocol） 
    /// 3，同时client中保存一份当前user的数据
    /// </summary>
    class LoginController : BaseController
    {
        public LoginController()
        {
            ContollerId = (int)ProtocolID.Login;
        }

        public override void HandleRequest(byte[] data, Client client, Server server)
        {
            base.HandleRequest(data, client,server);

            UserDAO userDAO = new UserDAO();
            LoginProtocol loginProtocol = new LoginProtocol();
            loginProtocol.Decode(data);           
            User user = userDAO.VerifyUser(client.MySQLConn, loginProtocol.Username, loginProtocol.Password);

            //用户存在则user信息存入client的user中,将结果和数据发送给客户端
            if (user == null)
            {
                Console.WriteLine("登陆失败");

                ResultProtocol resultProtocol = new ResultProtocol(false);
                server.SendResponse(resultProtocol.Encode(), client);
            }
            else
            {
                Console.WriteLine("登陆成功");  

                client.currentUser.Id = user.Id;
                client.currentUser.Username = user.Username;
                ResultProtocol resultProtocol = new ResultProtocol(true);
                server.SendResponse(resultProtocol.Encode(), client);
                ReturnUserDataProtocol returnUserDataProtocol = new ReturnUserDataProtocol(user.Id, user.Username);
                server.SendResponse(returnUserDataProtocol.Encode(), client);
            }
        }
    }
}
