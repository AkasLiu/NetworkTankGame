﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class User
    {
        public User(int id, string username, string passsword)
        {
            this.Id = id;
            this.Username = username;
            this.Password = passsword;
        }

        public User()
        {

        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
