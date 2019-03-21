using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHallState : StateBase
{
    public GameHallState()
    {
        currentState = StateType.GameHall;
    }

    public override void Enter()
    {
        Debug.Log("enter GameHall");
        GameHallController.Instance.ShowView();
        GameHallController.Instance.RegisterProto();
    }

    public override void Excute()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        Debug.Log("exit GameHall");
        GameHallController.Instance.CloseView();
        GameHallController.Instance.UnregisterProto();
    }
}
