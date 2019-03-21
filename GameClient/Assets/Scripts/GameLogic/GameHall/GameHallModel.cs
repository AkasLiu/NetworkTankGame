using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHallModel : ModelBase {

    public int ID { get; set;}
    public string Username { get; set; }
    public int Gold { get; set; }

    public GameHallModel()
    {
        ID = -1;
        Username = null;
        Gold = 0;
    }

}
