using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank {

    public int ID { get; set; }
    public int Hp { get; set; }
    public bool IsDie { get; set; }

    public GameObject SelfGameObject { get; set; }

    public Tank()
    {
        ID = -1;
        Hp = 100;
        IsDie = false;
    }

    public Tank(int id,GameObject gameObject)
    {
        ID = id;
        Hp = 100;
        SelfGameObject = gameObject;
        IsDie = false;
    }

}
