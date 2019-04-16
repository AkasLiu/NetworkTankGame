using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Servers;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("192.168.1.116", 6688);
            //Server server = new Server("192.168.2.136", 6688);
            server.Start();

            Console.ReadKey();
        }
    }
}
