using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;

namespace GameServer.Servers
{
    class Room
    {
        public int RoomID { get; set; }
        public int Max_count { get; set; }
        public int CurrentCount { get; set; }
        public List<Client> clientList;

        public Room()
        {
            CurrentCount = 0;
            Max_count = 2;
            clientList = new List<Client>();
            RoomID = 0;
        }

        public Room(int roomid)
        {
            CurrentCount = 0;
            Max_count = 2;
            clientList = new List<Client>();
            RoomID = roomid;
        }

    }
}
