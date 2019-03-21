using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginState : StateBase
{
    public LoginState()
    {
        currentState = StateType.Login;
    }

    public override void Enter()
    {
        Debug.Log("enter loginstate");        
        LoginController.Instance.ShowView();
        LoginController.Instance.RegisterProto();
    }

    public override void Excute()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("exit loginstate");        
        LoginController.Instance.CloseView();
        LoginController.Instance.UnregisterProto();
    }

}
