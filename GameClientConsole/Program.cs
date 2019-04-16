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
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.136"), 23456);

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

            while (true)
            {


                if(DateTime.Now.Ticks / 10000 - time > 20)
                {

                    Byte[] stream = new Byte[44];

                    List<byte> streamList = new List<byte>();

                    streamList.AddRange(BitConverter.GetBytes(44));
                    streamList.AddRange(BitConverter.GetBytes(1));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));
                    streamList.AddRange(BitConverter.GetBytes(0));



                    clientSocket.Send(streamList.ToArray());

                    time = DateTime.Now.Ticks / 10000;
                }



                
            }
        }
    }
}
