using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : ModelBase {

    public int ID { get; set; }
    public string Username { get; set; }
    public int Hp { get; set; }
    public int Level { get; set; }

    public BattleModel()
    {
        ID = -1;
        Username = null;
        Hp = 0;
        Level = 1;
    }



}
