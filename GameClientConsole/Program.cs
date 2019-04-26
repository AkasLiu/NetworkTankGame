using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;
using System.Net.Sockets;
using System.Net;

namespace GameClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            long time = 0 ;


            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.116"), 6688);

            try
            {
                clientSocket.Connect(serverIPEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }


            //CustomTransform simplyTransForm = new CustomTransform(10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f);
            //byte[] bytes = new StartGameProtocol(5, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f).Encode();

            //clientSocket.Send(bytes);

            LoginProtocol loginProtocol = new LoginProtocol("1", "1");
            clientSocket.Send(loginProtocol.Encode());

            byte[] buffer = new byte[1024];

            clientSocket.Receive(buffer);

            ReturnUserDataProtocol returnUserDataProtocol = new ReturnUserDataProtocol();
            returnUserDataProtocol.Decode(buffer);

            Console.WriteLine(returnUserDataProtocol.Username);

            Console.Read();

                
        }
    }
}
