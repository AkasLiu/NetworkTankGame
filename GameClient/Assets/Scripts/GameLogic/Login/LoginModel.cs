using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  LoginModel : ModelBase
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginModel()
    {
        ID = -1;
        Username = null;
        Password = null;
    }

}
