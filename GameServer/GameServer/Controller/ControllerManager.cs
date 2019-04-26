using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Controller
{
    class ControllerManager
    {
        private Dictionary<int, BaseController> controllerDict = new Dictionary<int, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }

        void InitController()
        {
            controllerDict.Add((int)ProtocolID.None, new BaseController());
            controllerDict.Add((int)ProtocolID.Login, new LoginController());
            controllerDict.Add((int)ProtocolID.Register, new RegisterController());
            controllerDict.Add((int)ProtocolID.StartGame, new StartGameController());
            controllerDict.Add((int)ProtocolID.SyncPosition,new SyncPositionController());
            controllerDict.Add((int)ProtocolID.Fire, new FireController());
            controllerDict.Add((int)ProtocolID.Die, new DieController());
            controllerDict.Add((int)ProtocolID.Revive, new ReviveController());
            controllerDict.Add((int)ProtocolID.ExitGame, new ExitGameController());
        }

        public void HandleRequest(int protocol_Id, byte[] data, Client client, Server server)
        {
            BaseController controller;
            //根据protocol_Id查到对应的controller
            bool isGet = controllerDict.TryGetValue(protocol_Id, out controller);
            //TODO 做成日志
            if (isGet == false)
            {
                Console.WriteLine("error，请求[" + protocol_Id + "]无法处理");
                return;
            }
            else
            {
                //stream = controller.HandleRequest(data, client,server);
                controller.HandleRequest(data, client, server);
            }
            //server.SendResponse(stream, client);
        }
    }
}
