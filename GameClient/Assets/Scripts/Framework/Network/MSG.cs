using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSG {

    public int Protocol_Id { get; set; }
    public byte[] Data { get; set; }

    public MSG(int protocol_id, byte[] data)
    {
        Protocol_Id = protocol_id;
        Data = data;
    }
}
