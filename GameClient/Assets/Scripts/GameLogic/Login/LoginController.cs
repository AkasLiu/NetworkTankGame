using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Protocol;
using Common;
using System;

public class LoginController : Singleton<LoginController>
{
    LoginView view;
    //UserData userData;
    //LoginModel model;

    public LoginController()
    {
        view = new LoginView();
        //model = new LoginModel(); 
        //userData = new UserData();
    }

    public void RegisterProto()
    {
        NetworkManager.Instance.RegisterProto((int)ProtocolId.ReturnUserDataProtocol, Debuglog);
        NetworkManager.Instance.RegisterProto((int)ProtocolId.ReturnUserDataProtocol, Verify);
    }

    public void UnregisterProto()
    {
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.ReturnUserDataProtocol, Debuglog);
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.ReturnUserDataProtocol, Verify);
    }

    public void Debuglog(object param)
    {
        Debug.Log("login");
    }

    public void Verify(object param)
    {
        byte[] data = (byte[])param;
        ReturnUserDataProtocol returnUserDataProtocol = new ReturnUserDataProtocol();
        returnUserDataProtocol.Decode(data);
        if (returnUserDataProtocol.Result)
        {
            //1，save userData
            //2,进入GameHall
            UserData userData = new UserData(returnUserDataProtocol.Id, returnUserDataProtocol.Username);
            PlayerInfoManager.Instance.SetUserData(userData);
            StateMachine.Instance.ChangeState(StateType.GameHall);
        }
        else
        {
            Debug.Log("fail");
        }
    }   

    public void Login(string username, string pwd)
    {
        //1,set Data to _model
        //2,NetManager发送请求       

        NetworkManager.Instance.Send(new LoginProtocol(username, pwd));
    }

    public void ShowView()
    {
        UIManager.Instance.AddView(view);        
    }

    public void CloseView()
    {
        UIManager.Instance.RemoveView(view);
    }


    
}
