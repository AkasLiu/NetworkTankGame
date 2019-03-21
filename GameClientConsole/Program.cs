using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Protocol;
using System.Net.Sockets;
using System.Net;
using Common;

namespace GameClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.121"), 6688);

            try
            {
                clientSocket.Connect(serverIPEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }


            CustomTransform simplyTransForm = new CustomTransform(10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f);
            byte[] bytes = new StartGameProtocol(5, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f).Encode();
            
            clientSocket.Send(bytes);

            while (true)
            {
                byte[] data = new byte[1024];
                int count = clientSocket.Receive(data);
                ReturnUserDataProtocol returnUserDataProtocol = new ReturnUserDataProtocol();
                returnUserDataProtocol.Decode(data);
            }
        }
    }
}
