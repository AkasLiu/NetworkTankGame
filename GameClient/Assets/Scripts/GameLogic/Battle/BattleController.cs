using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Protocol;
using Common;
using UnityEngine.EventSystems;

public class BattleController : Singleton<BattleController>
{
    //BattleModel model;
    BattleView view;

    public BattleController()
    {
        //model = new BattleModel();
        view = new BattleView();
    }

    #region 被绑定到button中的事件 
    /// <summary>
    /// 被绑定到button中，
    /// 当button按下时，发送数据给服务器
    /// </summary>
    public void Fire()
    {
        FireProtocol fireProtocol = new FireProtocol(PlayerInfoManager.Instance.GetUserData().ID);
        NetworkManager.Instance.Send(fireProtocol);
    }

    public void ExitGame()
    {
        //1,销毁游戏物体，将摄像机放到根世界
        //2，销毁当前view
        ExitGameProtocol exitGameProtocol = new ExitGameProtocol(PlayerInfoManager.Instance.GetUserData().ID);
        NetworkManager.Instance.Send(exitGameProtocol);
        StateMachine.Instance.ChangeState(StateType.GameHall);
    }
    #endregion

    public void RegisterProto()
    {
        NetworkManager.Instance.RegisterProto((int)ProtocolId.JoinGame, GenerateTank);
        NetworkManager.Instance.RegisterProto((int)ProtocolId.ExitGame, OthersExitGame);
    }

    public void UnregisterProto()
    {
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.JoinGame, GenerateTank);    
        NetworkManager.Instance.RegisterProto((int)ProtocolId.ExitGame, OthersExitGame);

    }

    public void GenerateTank(object param)
    {
        Debug.Log("创建坦克");
        byte[] data = (byte[])param;
        StartGameProtocol startGameProtocol = new StartGameProtocol();
        startGameProtocol.Decode(data);
        CustomTransform customTransform = new CustomTransform(startGameProtocol.CTF.X,
            startGameProtocol.CTF.Y, startGameProtocol.CTF.Z, startGameProtocol.CTF.RX, startGameProtocol.CTF.RY, startGameProtocol.CTF.RZ);
        SceneManager.Instance.GenerateTank(startGameProtocol.Role_Id, customTransform);
    }

    public void OthersExitGame(object param)
    {
        SceneManager.Instance.Die(param);
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
