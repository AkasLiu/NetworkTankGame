using System;
using System.Collections.Generic;
using System.Text;

public enum ProtocolId
{
    None = 1000,
    Login = 1001,
    Register = 1002,
    ReturnUserDataProtocol = 1003,
    StartGame = 1004,
    Move = 1005,
    SyncPosition = 1006,
    Fire = 1007,
    Damage =1008,
    Die =1009,
    Revive =1010,
    ExitGame = 1011,
    JoinGame = 1012,
}
