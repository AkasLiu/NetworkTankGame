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
        public MyTransform Mytf { get; set; }
        public int Hp { get; set; }

        public PlayerData()
        {

        }

        public PlayerData(int id,MyTransform mytf)
        {
            Id = id;
            Mytf = mytf;
        }

        public PlayerData(int id)
        {
            Id = id;
        }

    }
}
