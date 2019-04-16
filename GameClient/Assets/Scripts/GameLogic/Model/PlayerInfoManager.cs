using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : Singleton<PlayerInfoManager>
{
    UserData userData;

    public PlayerInfoManager()
    {
        userData = new UserData();        
    }

    public void SetUserData(UserData userData)
    {
        this.userData = userData;
    }

    public UserData GetUserData()
    {
        return userData;
    }

}
