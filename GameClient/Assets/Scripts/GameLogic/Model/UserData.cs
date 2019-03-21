using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData{

    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public UserData()
    {
        ID = -1;
        Username = null;
        Password = null;
    }

    public UserData(int id, string username)
    {
        ID = id;
        Username = username;
    }

}
