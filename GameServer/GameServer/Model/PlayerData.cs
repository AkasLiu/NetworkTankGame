using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Model
{
    class PlayerData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public CustomTransform stf { get; set; }
        public int hp { get; set; }


    }
}
