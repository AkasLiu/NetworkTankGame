using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : StateBase
{
    public BattleState()
    {
        currentState = StateType.Battle;
    }

    public override void Enter()
    {
        Debug.Log("enter Battle");
        BattleController.Instance.ShowView();
        BattleController.Instance.RegisterProto();
        SceneManager.Instance.Start();
    }

    public override void Excute()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        Debug.Log("exit Battle");
        BattleController.Instance.CloseView();
        BattleController.Instance.UnregisterProto();
        SceneManager.Instance.Destory();
    }
}
